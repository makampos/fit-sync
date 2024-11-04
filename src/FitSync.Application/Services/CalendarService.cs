using FitSync.Application.Extensions;
using FitSync.Domain.Dtos.CalendarEvents;
using FitSync.Domain.Interfaces;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.Extensions.Logging;

namespace FitSync.Application.Services;

public class CalendarService : ICalendarService
{
    private readonly ILogger<CalendarService> _logger;

    public CalendarService(ILogger<CalendarService> logger)
    {
        _logger = logger;
    }

    public Task<string> AddEventAsync(AddWorkoutPlanCalendarEventDto addWorkoutPlanCalendarEventsDto)
    {

        _logger.LogInformation("Adding new event to calendar");

        var calendar = new Calendar();

        addWorkoutPlanCalendarEventsDto.WorkoutPlansViewModel.ToList().ForEach(workoutPlan =>
        {
            var rrule = new RecurrencePattern(FrequencyType.Monthly) // monthly, weekly, daily
            {
                ByDay = new List<WeekDay> { new WeekDay((DayOfWeek)workoutPlan.Key) }, // Day of the week
                // Interval = 1,
                // Count = 1,
                Until = new DateTime(
                    addWorkoutPlanCalendarEventsDto.Until.Year,
                    addWorkoutPlanCalendarEventsDto.Until.Month,
                    addWorkoutPlanCalendarEventsDto.Until.Day) // end recurrence on 30th November 2024
            };

            var calendarEvent = new CalendarEvent()
            {
                Start = new CalDateTime(addWorkoutPlanCalendarEventsDto.StartDate.Year, addWorkoutPlanCalendarEventsDto.StartDate.Month, addWorkoutPlanCalendarEventsDto.StartDate.Day),
                // End = new CalDateTime(),
                Transparency = TransparencyType.Transparent,
                RecurrenceRules = new List<RecurrencePattern> { rrule },
                Summary =  WorkoutAggregator.Title(workoutPlan.Value), // Calendar event title
                Description = WorkoutAggregator.Workouts(workoutPlan.Value), // Calendar event description
                IsAllDay = true,
            };

            calendar.Events.Add(calendarEvent);
        });

        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        return Task.FromResult(serializedCalendar);
    }
}
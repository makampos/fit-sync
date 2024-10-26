using FitSync.Domain.Interfaces;
using FitSync.Domain.ViewModels;
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

    public async Task GetEventsAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<string> AddEventAsync(ICollection<WorkoutPlanViewModel> workoutPlansViewModel)
    {
        var workoutGroup = workoutPlansViewModel.GroupBy(x => x.Name);

        var calendar = new Calendar();

        workoutGroup.Select(t => t.Key).ToList().ForEach(day =>
        {
            // TODO: Implement recurrence rule based on user request
            // For each day, get the workout plans
            // and add them to the calendar
            // with a recurrence rule
            // that repeats every month on that day or week of the month (feature request)

            var workoutPlans = workoutPlansViewModel.Where(x => x.Name == day).ToList();
            workoutPlans.ForEach(workoutPlan =>
            {
                var rrule = new RecurrencePattern(FrequencyType.Monthly)
                {
                    ByDay = new List<WeekDay> { new WeekDay(DayOfWeek.Monday) },
                    // Interval = 1,
                    // Count = 1,
                    Until = new DateTime(2024, 11, 30)
                };

                var calendarEvent = new CalendarEvent()
                {
                    Start = new CalDateTime(new DateTime(2024, 11, 1)),
                    Transparency = TransparencyType.Transparent,
                    RecurrenceRules = new List<RecurrencePattern> { rrule },
                    Summary = workoutPlan.Name, // Workout Plan Title
                    Description = workoutPlan.WorkoutViewModels.Select(x => x.Title).Aggregate((i, j) => i + ", " + j), // Workout Plan title
                    IsAllDay = true,
                };

                calendar.Events.Add(calendarEvent);
            });
        });

        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        return  Task.FromResult(serializedCalendar);
    }

    public async Task UpdateEventAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteEventAsync(string eventId)
    {
        throw new NotImplementedException();
    }
}
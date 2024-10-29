using FitSync.Domain.Dtos.CalendarEvents;

namespace FitSync.Domain.Interfaces;

public interface ICalendarService
{
    Task<string> AddEventAsync(AddWorkoutPlanCalendarEventDto addWorkoutPlanCalendarEventsDto);
}
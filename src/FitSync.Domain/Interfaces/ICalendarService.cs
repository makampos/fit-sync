using FitSync.Domain.Dtos;

namespace FitSync.Domain.Interfaces;

public interface ICalendarService
{
    Task GetEventsAsync(DateTime startDate, DateTime endDate);
    Task<string> AddEventAsync(WorkoutPlanCalendarEvent workoutPlanCalendarEvents);
    Task UpdateEventAsync();
    Task DeleteEventAsync(string eventId);
}
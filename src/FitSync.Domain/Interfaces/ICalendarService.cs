using FitSync.Domain.Dtos;

namespace FitSync.Domain.Interfaces;

public interface ICalendarService
{
    Task<string> AddEventAsync(WorkoutPlanCalendarEvent workoutPlanCalendarEvents);
}
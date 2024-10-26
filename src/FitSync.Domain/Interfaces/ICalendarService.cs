using FitSync.Domain.ViewModels;

namespace FitSync.Domain.Interfaces;

public interface ICalendarService
{
    Task GetEventsAsync(DateTime startDate, DateTime endDate);
    Task<string> AddEventAsync(ICollection<WorkoutPlanViewModel> workoutPlansViewModel);
    Task UpdateEventAsync();
    Task DeleteEventAsync(string eventId);
}
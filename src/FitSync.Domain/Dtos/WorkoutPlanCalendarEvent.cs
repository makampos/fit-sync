using FitSync.Domain.ViewModels;

namespace FitSync.Domain.Dtos;

public record WorkoutPlanCalendarEvent(IDictionary<int, IReadOnlyCollection<WorkoutPlanViewModel>> WorkoutPlansViewModel, DateOnly StartDate, DateOnly Until);



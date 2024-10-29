using FitSync.Domain.ViewModels.WorkoutPlans;

namespace FitSync.Domain.Dtos.CalendarEvents;

public record AddWorkoutPlanCalendarEventDto(IDictionary<int, IReadOnlyCollection<WorkoutPlanViewModel>> WorkoutPlansViewModel,
    DateOnly StartDate, DateOnly Until);



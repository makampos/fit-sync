using FitSync.Domain.ViewModels.WorkoutPlans;

namespace FitSync.Domain.Dtos.CalendarEvents;

public record AddWorkoutPlanCalendarEventDto(
    IDictionary<int, IReadOnlyCollection<WorkoutPlanViewModel>> WorkoutPlansViewModel,
    DateOnly StartDate,
    DateOnly Until)
{
    public static AddWorkoutPlanCalendarEventDto Create(
        IDictionary<int, IReadOnlyCollection<WorkoutPlanViewModel>> workoutPlansViewModel,
        DateOnly startDate,
        DateOnly until)
    {
        return new AddWorkoutPlanCalendarEventDto(workoutPlansViewModel, startDate, until);
    }
}



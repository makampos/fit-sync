using FitSync.Domain.ViewModels.Workouts;

namespace FitSync.Domain.ViewModels.WorkoutPlans;

public record WorkoutPlanViewModel(int WorkoutPlanId, string Name, bool IsActive, ICollection<WorkoutWithExercisesSetViewModel>
        WorkoutWithExercisesSetViewModel)
{
    public static WorkoutPlanViewModel Create(int workoutPlanId, string name,
        bool isActive, ICollection<WorkoutWithExercisesSetViewModel> workoutWithExercisesSetViewModel)
    {
        return new WorkoutPlanViewModel(workoutPlanId, name, isActive, workoutWithExercisesSetViewModel);
    }
}

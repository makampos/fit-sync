using FitSync.Domain.ViewModels.Workouts;

namespace FitSync.Domain.ViewModels.WorkoutPlans;

public record WorkoutPlanViewModel(int WorkoutPlanId, string Name, ICollection<WorkoutWithExercisesSetViewModel> WorkoutWithExercisesSetViewModel)
{
    public static WorkoutPlanViewModel Create(int workoutPlanId, string name, ICollection<WorkoutWithExercisesSetViewModel> workoutWithExercisesSetViewModel)
    {
        return new WorkoutPlanViewModel(workoutPlanId, name, workoutWithExercisesSetViewModel);
    }
}

using FitSync.Domain.ViewModels.Workouts;

namespace FitSync.Domain.ViewModels.WorkoutPlans;

public record WorkoutPlanViewModel(int Id, string Name, ICollection<WorkoutWithExercisesSetViewModel> WorkoutWithExercisesSetViewModel)
{
    public static WorkoutPlanViewModel Create(int id, string name, ICollection<WorkoutWithExercisesSetViewModel> workoutWithExercisesSetViewModel)
    {
        return new WorkoutPlanViewModel(id, name, workoutWithExercisesSetViewModel);
    }
}

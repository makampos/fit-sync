namespace FitSync.Domain.ViewModels;

public record WorkoutPlanViewModel(int Id, string Name, ICollection<WorkoutViewModel> WorkoutsViewModel)
{
    public static WorkoutPlanViewModel Create(int id, string name, ICollection<WorkoutViewModel> workoutsViewModel)
    {
        return new WorkoutPlanViewModel(id, name, workoutsViewModel);
    }
}

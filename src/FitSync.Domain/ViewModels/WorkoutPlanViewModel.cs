namespace FitSync.Domain.ViewModels;

public record WorkoutPlanViewModel(int Id, string Name, ICollection<WorkoutViewModel> WorkoutViewModels);
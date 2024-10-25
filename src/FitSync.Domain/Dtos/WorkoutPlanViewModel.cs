namespace FitSync.Domain.Dtos;

public record WorkoutPlanViewModel(int WorkoutPlanId, string Name, ICollection<WorkoutDto> Workouts);
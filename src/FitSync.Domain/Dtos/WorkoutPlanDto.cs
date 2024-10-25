namespace FitSync.Domain.Dtos;

public record WorkoutPlanDto(int Id, string Name, int UserId, ICollection<WorkoutPlanWorkoutDto> Workouts);
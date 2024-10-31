namespace FitSync.Domain.Dtos.WorkoutPlans;

public record UpdateWorkoutPlanDto(int Id, int UserId, string Name, IDictionary<int, ExerciseSet> Workouts);
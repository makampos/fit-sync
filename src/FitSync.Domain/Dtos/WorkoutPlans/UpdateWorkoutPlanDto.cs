namespace FitSync.Domain.Dtos.WorkoutPlans;

public record UpdateWorkoutPlanDto(int Id, int UserId, string Name, IDictionary<int, ExerciseSet> Workouts)
{
    public static UpdateWorkoutPlanDto Create(int id, int userId, string name, IDictionary<int, ExerciseSet> workouts)
    {
        return new UpdateWorkoutPlanDto(id, userId, name, workouts);
    }
}
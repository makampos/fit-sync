namespace FitSync.Domain.Dtos.WorkoutPlans;


public record AddWorkoutPlanDto(int UserId, string Name, IDictionary<int, ExerciseSet> Workouts)
{
    public static AddWorkoutPlanDto Create(int userId, string name, IDictionary<int, ExerciseSet> workouts)
    {
        return new AddWorkoutPlanDto(userId, name, workouts);
    }
}

public record ExerciseSet(
    int Sets,
    int RepsMin,
    int RepsMax,
    int RestBetweenSets = 60, // in seconds
    string? Notes = null)
{
    public static ExerciseSet Create(
        int sets,
        int repsMin,
        int repsMax,
        int restBetweenSets = 60,
        string? notes = null)
    {
        return new ExerciseSet(sets, repsMin, repsMax, restBetweenSets, notes);
    }
}
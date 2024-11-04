namespace FitSync.Domain.Dtos.WorkoutPlans;

/// <summary>
/// Represents a workout plan that includes user information,
/// a name for the plan, and a collection of exercise sets.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the workout plan.</param>
/// <param name="Name">The name of the workout plan.</param>
/// <param name="WorkoutIdToExerciseSet">A dictionary mapping workout IDs to their corresponding exercise sets.</param>
public record AddWorkoutPlanDto(
    int UserId,
    string Name,
    IDictionary<int, ExerciseSet> WorkoutIdToExerciseSet)
{
    /// <summary>
    /// Creates a new instance of the <see cref="AddWorkoutPlanDto"/> class.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="name">The name of the workout plan.</param>
    /// <param name="workoutIdToExerciseSet">A dictionary mapping workout IDs to their corresponding exercise sets.</param>
    /// <returns>A new instance of <see cref="AddWorkoutPlanDto"/>.</returns>
    public static AddWorkoutPlanDto Create(int userId, string name, IDictionary<int, ExerciseSet> workoutIdToExerciseSet)
    {
        return new AddWorkoutPlanDto(userId, name, workoutIdToExerciseSet);
    }
}

/// <summary>
/// Represents a set of exercises with details about the number of sets,
/// repetitions, weight, rest time, and optional notes.
/// </summary>
/// <param name="Sets">The number of sets to perform in this exercise set.</param>
/// <param name="RepsMin">The minimum number of repetitions for this exercise set.</param>
/// <param name="RepsMax">The maximum number of repetitions for this exercise set.</param>
/// <param name="Weight">The weight to be used for this exercise set.</param>
/// <param name="RestBetweenSets">The rest time between sets in seconds (default is 60 seconds).</param>
/// <param name="Notes">Optional notes regarding the exercise set.</param>
public record ExerciseSet(
    int Sets,
    int RepsMin,
    int RepsMax,
    int Weight,
    int RestBetweenSets = 60, // in seconds
    string? Notes = null)
{
    /// <summary>
    /// Creates a new instance of the <see cref="ExerciseSet"/> class.
    /// </summary>
    /// <param name="sets">The number of sets to perform in this exercise set.</param>
    /// <param name="repsMin">The minimum number of repetitions for this exercise set.</param>
    /// <param name="repsMax">The maximum number of repetitions for this exercise set.</param>
    /// <param name="weight">The weight to be used for this exercise set.</param>
    /// <param name="restBetweenSets">The rest time between sets in seconds (default is 60 seconds).</param>
    /// <param name="notes">Optional notes regarding the exercise set.</param>
    /// <returns>A new instance of <see cref="ExerciseSet"/>.</returns>
    public static ExerciseSet Create(
        int sets,
        int repsMin,
        int repsMax,
        int weight,
        int restBetweenSets = 60,
        string? notes = null)
    {
        return new ExerciseSet(sets, repsMin, repsMax, weight, restBetweenSets, notes);
    }
}
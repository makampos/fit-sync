using System.Text;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Enums;

namespace FitSync.Domain.ViewModels.Workouts;

public record WorkoutWithExercisesSetViewModel(
    int WorkoutId,
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level,
    ExerciseSet ExerciseSet)
{
    public static WorkoutWithExercisesSetViewModel Create(
        int workoutId,
        string title,
        string description,
        WorkoutType type,
        string bodyPart,
        string equipment,
        WorkoutLevel level,
        ExerciseSet exerciseSet)
    {
        return new WorkoutWithExercisesSetViewModel(workoutId, title, description, type, bodyPart, equipment, level, exerciseSet);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"{Title}");
        builder.AppendLine($"{Description}");
        builder.AppendLine();
        return builder.ToString();
    }
}


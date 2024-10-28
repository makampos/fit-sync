using System.Text;
using FitSync.Domain.Enums;
using FitSync.Domain.Features.WorkoutPlans;

namespace FitSync.Domain.ViewModels;

public record WorkoutViewModel(
    int Id,
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level,
    ExerciseSet ExerciseSet)
{
    public static WorkoutViewModel Create(
        int id,
        string title,
        string description,
        WorkoutType type,
        string bodyPart,
        string equipment,
        WorkoutLevel level,
        ExerciseSet exerciseSet)
    {
        return new WorkoutViewModel(id, title, description, type, bodyPart, equipment, level, exerciseSet);
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


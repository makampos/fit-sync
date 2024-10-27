using System.Text;
using FitSync.Domain.Enums;

namespace FitSync.Domain.ViewModels;

public record WorkoutViewModel(
    int Id,
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level)
{
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"{Title}");
        builder.AppendLine($"{Description}");
        builder.AppendLine();
        return builder.ToString();
    }
}


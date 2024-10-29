using FitSync.Domain.Enums;

namespace FitSync.Domain.ViewModels.Workouts;

public record WorkoutViewModel(int Id, string Title, string Description, WorkoutType Type, string BodyPart,
    string Equipment, WorkoutLevel Level)
{
    public static WorkoutViewModel Create(int id, string title, string description, WorkoutType type, string bodyPart,
        string equipment, WorkoutLevel level)
    {
        return new WorkoutViewModel(id, title, description, type, bodyPart, equipment, level);
    }
}
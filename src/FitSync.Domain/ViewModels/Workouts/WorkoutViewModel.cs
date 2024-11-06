using FitSync.Domain.Enums;

namespace FitSync.Domain.ViewModels.Workouts;

public record WorkoutViewModel(
    int WorkoutId,
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level)
{
    public static WorkoutViewModel Create(int workoutId, string title, string description, WorkoutType type, string bodyPart,
        string equipment, WorkoutLevel level)
    {
        return new WorkoutViewModel(workoutId, title, description, type, bodyPart, equipment, level);
    }
}
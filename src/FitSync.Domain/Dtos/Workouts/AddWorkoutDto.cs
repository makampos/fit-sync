using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos.Workouts;

public record AddWorkoutDto(
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level)
{
    public static AddWorkoutDto Create(string title, string description, WorkoutType type, string bodyPart,
        string equipment, WorkoutLevel level)
    {
        return new AddWorkoutDto(title, description, type, bodyPart, equipment, level);
    }
}
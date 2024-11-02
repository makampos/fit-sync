using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos.Workouts;

public record UpdateWorkoutDto(
    int Id,
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level)
{
    public static UpdateWorkoutDto Create(int id, string title, string description, WorkoutType type, string bodyPart,
        string equipment, WorkoutLevel level)
    {
        return new UpdateWorkoutDto(id, title, description, type, bodyPart, equipment, level);
    }
}
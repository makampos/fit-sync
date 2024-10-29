using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos.Workouts;

public record AddWorkoutDto(
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level);
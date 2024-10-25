using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos;

public record WorkoutDto(
    int Id,
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level);
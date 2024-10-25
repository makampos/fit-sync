using FitSync.Domain.Enums;

namespace FitSync.Domain.ViewModels;

public record WorkoutViewModel(
    int Id,
    string Title,
    string Description,
    WorkoutType Type,
    string BodyPart,
    string Equipment,
    WorkoutLevel Level);
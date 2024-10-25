using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos;

public record UserDto(string Name, int Age, Genre Genre, ICollection<WorkoutPlanDto>? WorkoutPlans = null);
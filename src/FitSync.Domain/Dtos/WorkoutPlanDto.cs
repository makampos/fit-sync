using System.Text.Json.Serialization;

namespace FitSync.Domain.Dtos;

public record WorkoutPlanDto(
    [property: JsonIgnore] int Id,
    string Name,
    int UserId, ICollection<WorkoutPlanWorkoutDto> WorkoutPlans);
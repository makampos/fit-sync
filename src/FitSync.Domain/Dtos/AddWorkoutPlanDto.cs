namespace FitSync.Domain.Dtos;

public record AddWorkoutPlanDto(
    string Name,
    int UserId, ICollection<WorkoutPlanWorkoutDto> WorkoutsDto);
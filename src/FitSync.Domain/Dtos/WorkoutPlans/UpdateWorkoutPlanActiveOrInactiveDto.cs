namespace FitSync.Domain.Dtos.WorkoutPlans;

public record UpdateWorkoutPlanActiveOrInactiveDto(int WorkoutPlanId, bool IsActive)
{
    public static UpdateWorkoutPlanActiveOrInactiveDto Create(int workoutPlanId, bool isActive)
    {
        return new UpdateWorkoutPlanActiveOrInactiveDto(workoutPlanId, isActive);
    }
}
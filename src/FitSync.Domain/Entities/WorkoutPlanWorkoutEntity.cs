namespace FitSync.Domain.Entities;

public class WorkoutPlanWorkoutEntity : TrackableEntity
{
    public int WorkoutPlanId { get; set; }
    public int WorkoutId { get; set; }

    // Navigation properties
    public virtual WorkoutPlanEntity WorkoutPlan { get; set; }
    public virtual WorkoutEntity Workout { get; set; }
}
namespace FitSync.Domain.Entities;

public class WorkoutPlanEntity : TrackableEntity
{
    public string Name { get; set; }

    // Foreign key
    public int UserId { get; set; }

    // Navigation property
    public virtual UserEntity User { get; set; }

    // Navigation property for many-to-many relationship
    public virtual ICollection<WorkoutPlanWorkoutEntity> WorkoutPlanWorkoutEntities { get; set; } = new List<WorkoutPlanWorkoutEntity>();
}
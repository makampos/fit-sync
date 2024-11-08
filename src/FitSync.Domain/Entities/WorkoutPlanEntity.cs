namespace FitSync.Domain.Entities;

public class WorkoutPlanEntity : TrackableEntity
{
    public string Name { get; set; }

    // Foreign key
    public int UserId { get; set; }
    public bool IsActive { get; private set; } = false;

    // Navigation property
    public virtual UserEntity User { get; set; }

    // Navigation property for many-to-many relationship
    public virtual ICollection<WorkoutPlanWorkoutEntity> WorkoutPlanWorkoutEntities { get; set; } = new List<WorkoutPlanWorkoutEntity>();

    public void Update(string name)
    {
        Name = name;
    }

    public void ToggleIsActive(bool isActive)
    {
        IsActive = isActive;
    }
}
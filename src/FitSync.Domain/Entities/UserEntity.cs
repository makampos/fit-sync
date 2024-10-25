using FitSync.Domain.Enums;

namespace FitSync.Domain.Entities;

// TODO: Create user Profile entity
public class UserEntity : TrackableEntity
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Genre Genre { get; set; }

    public virtual ICollection<WorkoutPlanEntity> WorkoutPlans { get; set; } = new List<WorkoutPlanEntity>();
}
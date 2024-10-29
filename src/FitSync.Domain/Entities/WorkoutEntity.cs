using FitSync.Domain.Enums;

namespace FitSync.Domain.Entities;

public class WorkoutEntity : TrackableEntity
{
    public WorkoutEntity() { }

    public string Title { get; set; }
    public string Description { get; set; }
    public WorkoutType Type { get; set; }
    public string BodyPart { get; set; }
    public string Equipment { get; set; }
    public WorkoutLevel WorkoutLevel { get; set; }

    public virtual ICollection<WorkoutPlanWorkoutEntity> WorkoutPlans { get; set; }

    public void Update(string title, string description, WorkoutType type, string bodyPart, string equipment, WorkoutLevel level)
    {
        Title = title;
        Description = description;
        Type = type;
        BodyPart = bodyPart;
        Equipment = equipment;
        WorkoutLevel = level;
    }
}
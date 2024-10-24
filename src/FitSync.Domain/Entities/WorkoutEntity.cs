using FitSync.Domain.Enums;

namespace FitSync.Domain.Entities;

public class WorkoutEntity : TrackableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public WorkoutType Type { get; set; }
    public string BodyPart { get; set; }
    public string Equipment { get; set; }
    public WorkoutLevel WorkoutLevel { get; set; }
}
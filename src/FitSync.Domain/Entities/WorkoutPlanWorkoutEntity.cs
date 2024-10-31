namespace FitSync.Domain.Entities;

public class WorkoutPlanWorkoutEntity : TrackableEntity
{
    public WorkoutPlanWorkoutEntity(){ }

    private WorkoutPlanWorkoutEntity(int workoutId, int sets, int repsMin, int repsMax, int restBetweenSets, string? notes)
    {
        WorkoutId = workoutId;
        Sets = sets;
        RepsMin = repsMin;
        RepsMax = repsMax;
        RestBetweenSets = restBetweenSets;
        Notes = notes;
    }

    private WorkoutPlanWorkoutEntity(int workoutId)
    {
        WorkoutId = WorkoutId;
    }

    public int WorkoutPlanId { get;  set; }
    public int WorkoutId { get;  set; }

    public int Sets { get; private set; }
    public int RepsMin { get; private set; }
    public int RepsMax { get; private set; }
    public int RestBetweenSets { get; private set; }
    public string? Notes { get; private set; }

    // Navigation properties
    public virtual WorkoutPlanEntity WorkoutPlan { get; set; }
    public virtual WorkoutEntity Workout { get; set; }

    public static WorkoutPlanWorkoutEntity Create(int workoutId, int sets, int repsMin, int repsMax, int restBetweenSets, string? notes)
    {
        return new WorkoutPlanWorkoutEntity(workoutId, sets, repsMin, repsMax, restBetweenSets, notes);
    }

    public void Update(int workoutId, int sets, int repsMin, int repsMax, int restBetweenSets, string? notes)
    {
        WorkoutId = workoutId;
        Sets = sets;
        RepsMin = repsMin;
        RepsMax = repsMax;
        RestBetweenSets = restBetweenSets;
        Notes = notes;
    }
}
using FitSync.Domain.Entities;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutPlanWorkoutRepository : IRepository<WorkoutPlanWorkoutEntity>
{
    Task<IReadOnlyCollection<WorkoutPlanWorkoutEntity>> GetWorkoutPlanWorkoutsByWorkoutPlanIdAsync(int workoutPlanId);
}
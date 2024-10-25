using FitSync.Domain.Entities;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutPlanRepository : IRepository<WorkoutPlanEntity>
{
  Task<WorkoutPlanEntity?> GetWorkoutPlanIncludedWorkoutsAsync(int workoutPlanId);
  Task<IReadOnlyCollection<WorkoutPlanEntity>> GetWorkoutPlanIncludedWorkoutsByUserIdAsync(int userId);
}
using FitSync.Domain.Entities;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutPlanRepository : IRepository<WorkoutPlanEntity>
{
  Task<WorkoutPlanEntity?> GetWorkoutPlanWithWorkoutsAsync(int workoutPlanId);
}
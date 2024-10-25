using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Repositories;

public class WorkoutPlanRepository(FitSyncDbContext fitSyncDbContext)
    : Repository<WorkoutPlanEntity>(fitSyncDbContext), IWorkoutPlanRepository
{
    public async Task<WorkoutPlanEntity?> GetWorkoutPlanWithWorkoutsAsync(int workoutPlanId)
    {
        return await fitSyncDbContext.WorkoutPlans
            .Include(wp => wp.Workouts)
            .FirstOrDefaultAsync(wp => wp.Id == workoutPlanId);
    }
}
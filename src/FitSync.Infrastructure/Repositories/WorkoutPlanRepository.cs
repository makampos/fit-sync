using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Repositories;

public class WorkoutPlanRepository(FitSyncDbContext fitSyncDbContext)
    : Repository<WorkoutPlanEntity>(fitSyncDbContext), IWorkoutPlanRepository
{
    public async Task<WorkoutPlanEntity?> GetWorkoutPlanIncludedWorkoutsAsync(int workoutPlanId)
    {
        return await SetAsTracking
            .Include(wp => wp.WorkoutPlanWorkoutEntities)
            .ThenInclude(wp => wp.Workout)
            .FirstOrDefaultAsync(wp => wp.Id == workoutPlanId);
    }

    public async Task<IReadOnlyCollection<WorkoutPlanEntity>> GetWorkoutPlanIncludedWorkoutsByUserIdAsync(int userId)
    {
        return await SetAsTracking
            .Include(wp => wp.WorkoutPlanWorkoutEntities)
            .ThenInclude(wp => wp.Workout)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }
}
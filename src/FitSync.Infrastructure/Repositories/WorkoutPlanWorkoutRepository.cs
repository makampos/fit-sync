using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Repositories;

public class WorkoutPlanWorkoutRepository(FitSyncDbContext fitSyncDbContext)
    : Repository<WorkoutPlanWorkoutEntity>(fitSyncDbContext), IWorkoutPlanWorkoutRepository
{
    public async Task<IReadOnlyCollection<WorkoutPlanWorkoutEntity>> GetWorkoutPlanWorkoutsByWorkoutPlanIdAsync(int workoutPlanId)
    {
        // 1.N
        var wpw = await SetAsTracking
            .Include(wpw => wpw.Workout)
            .Include(wpw => wpw.WorkoutPlan)
            .Where(x => x.WorkoutPlanId == workoutPlanId)
            .ToListAsync();

        return wpw;
    }
}
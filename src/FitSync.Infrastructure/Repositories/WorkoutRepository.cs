using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Repositories;

public class WorkoutRepository(FitSyncDbContext fitSyncDbContext)
    : Repository<WorkoutEntity>(fitSyncDbContext), IWorkoutRepository
{
    public async Task<IEnumerable<WorkoutEntity>> GetWorkoutsByIdsAsync(IEnumerable<int> ids)
    {
        return await fitSyncDbContext.Workouts
            .Where(w => ids.Contains(w.Id))
            .ToListAsync();
    }
}
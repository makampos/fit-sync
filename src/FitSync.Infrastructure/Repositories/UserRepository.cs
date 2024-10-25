using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Repositories;

public class UserRepository(FitSyncDbContext fitSyncDbContext)
    : Repository<UserEntity>(fitSyncDbContext), IUserRepository
{
    public async Task<UserEntity?> GetUserByIdIncludeAllAsync(int id)
    {
        return await SetAsTracking
            .Include(u => u.WorkoutPlans)
            .ThenInclude(wp => wp.Workouts)
            .ThenInclude(w => w.Workout)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}

using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;

namespace FitSync.Infrastructure.Repositories;

public class WorkoutRepository(FitSyncDbContext fitSyncDbContext)
    : Repository<WorkoutEntity>(fitSyncDbContext), IWorkoutRepository
{

}
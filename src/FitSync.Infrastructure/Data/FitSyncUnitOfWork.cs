using FitSync.Domain.Interfaces;

namespace FitSync.Infrastructure.Data;

public class FitSyncUnitOfWork(
    FitSyncDbContext fitSyncDbContext,
    IWorkoutRepository workoutRepository) : UnitOfWork(fitSyncDbContext), IFitSyncUnitOfWork
{
    public IWorkoutRepository WorkoutRepository { get; init; } = workoutRepository;

}
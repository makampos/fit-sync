using FitSync.Domain.Interfaces;

namespace FitSync.Infrastructure.Data;

public class FitSyncUnitOfWork(
    FitSyncDbContext fitSyncDbContext,
    IWorkoutRepository workoutRepository,
    IUserRepository userRepository) : UnitOfWork(fitSyncDbContext), IFitSyncUnitOfWork
{
    public IWorkoutRepository WorkoutRepository { get; init; } = workoutRepository;
    public IUserRepository UserRepository { get; init; } = userRepository;

}
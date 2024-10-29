using FitSync.Domain.Interfaces;

namespace FitSync.Infrastructure.Data;

public class FitSyncUnitOfWork(
    FitSyncDbContext fitSyncDbContext,
    IWorkoutRepository workoutRepository,
    IUserRepository userRepository,
    IWorkoutPlanRepository workoutPlanRepository,
    IWorkoutPlanWorkoutRepository workoutPlanWorkoutRepository) : UnitOfWork(fitSyncDbContext), IFitSyncUnitOfWork
{
    public IWorkoutRepository WorkoutRepository { get; init; } = workoutRepository;
    public IUserRepository UserRepository { get; init; } = userRepository;
    public IWorkoutPlanRepository WorkoutPlanRepository { get; init; } = workoutPlanRepository;
    public IWorkoutPlanWorkoutRepository WorkoutPlanWorkoutRepository { get; init; } = workoutPlanWorkoutRepository;

}
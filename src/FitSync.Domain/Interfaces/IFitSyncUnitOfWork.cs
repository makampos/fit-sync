namespace FitSync.Domain.Interfaces;

public interface IFitSyncUnitOfWork : IUnitOfWork
{
    IWorkoutRepository WorkoutRepository { get; init; }
    IUserRepository UserRepository { get; init; }
    IWorkoutPlanRepository WorkoutPlanRepository { get; init; }

    IWorkoutPlanWorkoutRepository WorkoutPlanWorkoutRepository { get; init; }
}
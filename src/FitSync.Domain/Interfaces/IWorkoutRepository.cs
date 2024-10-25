using FitSync.Domain.Entities;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutRepository : IRepository<WorkoutEntity>
{
    Task<IEnumerable<WorkoutEntity>> GetWorkoutsByIdsAsync(IEnumerable<int> ids);
}
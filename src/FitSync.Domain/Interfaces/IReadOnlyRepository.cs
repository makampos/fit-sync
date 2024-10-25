using FitSync.Domain.Entities;
using FitSync.Domain.Enums;
using FitSync.Domain.Results;

namespace FitSync.Domain.Interfaces;

public interface IReadOnlyRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<TEntity>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<WorkoutEntity>> GetFilteredWorkoutsAsync(
        WorkoutType? type = null,
        string? bodyPart = null,
        string? equipment = null,
        WorkoutLevel? level = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}
using FitSync.Domain.Entities;
using FitSync.Domain.Enums;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Results;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Data;

public class ReadOnlyRepository<TEntity>(
    FitSyncDbContext fitSyncDbContext
) : RepositoryProperties<TEntity>(fitSyncDbContext), IReadOnlyRepository<TEntity> where TEntity : Entity
{
    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await SetAsTracking.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedResult<TEntity>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await SetAsNoTracking.CountAsync(cancellationToken);
        var items = await SetAsNoTracking
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return  new PagedResult<TEntity>(items, totalCount, pageSize, pageNumber);
    }

    //TODO: Move to a more specific repository and Apply the Specification pattern
    public async Task<PagedResult<WorkoutEntity>> GetFilteredWorkoutsAsync(
        WorkoutType? type = null,
        string? bodyPart = null,
        string? equipment = null,
        WorkoutLevel? level = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = SetAsNoTracking.OfType<WorkoutEntity>();

        if (type.HasValue)
        {
            query = query.Where(w => w.Type == type.Value);
        }

        if (!string.IsNullOrEmpty(bodyPart))
        {
            query = query.Where(w => EF.Functions.ILike(w.BodyPart, bodyPart));
        }

        if (!string.IsNullOrEmpty(equipment))
        {
            query = query.Where(w => EF.Functions.ILike(w.Equipment, equipment));
        }

        if (level.HasValue)
        {
            query = query.Where(w => w.WorkoutLevel == level.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<WorkoutEntity>(items, totalCount, pageSize, pageNumber);
    }
}
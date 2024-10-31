using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;

namespace FitSync.Infrastructure.Data;

public class Repository<TEntity>(
    FitSyncDbContext fitSyncDbContext
) : ReadOnlyRepository<TEntity>(fitSyncDbContext),
    IRepository<TEntity> where TEntity : Entity
{
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is TrackableEntity trackable)
        {
            trackable.Created("admin");
        }

        await Set.AddAsync(entity, cancellationToken);
    }

    public async Task AddAsync(IEnumerator<TEntity> entities, CancellationToken cancellationToken = default)
    {
        while (entities.MoveNext())
        {
            if (entities.Current is TrackableEntity trackable)
            {
                trackable.Created("admin");
            }
        }

        await Set.AddRangeAsync(entities as IEnumerable<TEntity> ?? Array.Empty<TEntity>(), cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is TrackableEntity trackable)
        {
            trackable.Updated("admin");
        }

        await Task.Run(() =>
        {
            Set.Update(entity);
        }, cancellationToken);
    }

    public async Task UpdateRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities.Any(e => e is TrackableEntity))
        {
            foreach (var entity in entities)
            {
                if (entity is TrackableEntity trackable)
                {
                    trackable.Updated("admin");
                }
            }
        }

        await Task.Run(() =>
        {
            Set.UpdateRange(entities);
        }, cancellationToken);
    }

    public async Task DeleteRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities.Any(e => e is TrackableEntity))
        {
            foreach (var entity in entities)
            {
                if (entity is TrackableEntity trackable)
                {
                    trackable.Deleted("admin");

                    await Task.Run(() =>
                    {
                        Set.Update(entity);
                    }, cancellationToken);
                }
                else
                {
                    await Task.Run(() =>
                    {
                        Set.Remove(entity);
                    }, cancellationToken);
                }
            }
        }

        await Task.Run(() =>
        {
            Set.RemoveRange(entities);
        }, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is TrackableEntity trackable)
        {
            trackable.Deleted("admin");

            await Task.Run(() =>
            {
                Set.Update(entity);
            }, cancellationToken);
        }
        else
        {
            await Task.Run(() =>
            {
                Set.Remove(entity);
            }, cancellationToken);
        }
    }
}
using FitSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Data;

public class RepositoryProperties<TEntity>(
    FitSyncDbContext fitSyncDbContext
) where TEntity : Entity
{
    protected readonly FitSyncDbContext ApplicationDbContext = fitSyncDbContext;

    protected DbSet<TEntity> Set => ApplicationDbContext.Set<TEntity>();

    protected IQueryable<TEntity> SetAsTracking
    {
        get
        {
            var query = Set.AsTracking();

            if (typeof(TEntity).IsSubclassOf(typeof(TrackableEntity)))
            {
                query = query.Where(e => !(e as TrackableEntity)!.IsDeleted);
            }

            return query;
        }
    }

    protected IQueryable<TEntity> SetAsNoTracking
    {
        get
        {
            var query = Set.AsNoTracking();

            if (typeof(TEntity).IsSubclassOf(typeof(TrackableEntity)))
            {
                query = query.Where(e => !(e as TrackableEntity)!.IsDeleted);
            }

            return query;
        }
    }
}
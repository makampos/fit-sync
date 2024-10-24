using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;

namespace FitSync.Infrastructure.Data;

public class ReadOnlyRepository<TEntity>(
    FitSyncDbContext fitSyncDbContext
) : RepositoryProperties<TEntity>(fitSyncDbContext), IReadOnlyRepository<TEntity> where TEntity : Entity
{

}
using FitSync.Domain.Interfaces;

namespace FitSync.Infrastructure.Data;

public class UnitOfWork(FitSyncDbContext fitSyncDbContext) : IUnitOfWork
{
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await fitSyncDbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
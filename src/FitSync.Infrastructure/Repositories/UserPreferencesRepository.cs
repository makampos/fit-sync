using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;

namespace FitSync.Infrastructure.Repositories;

public class UserPreferencesRepository(FitSyncDbContext fitSyncDbContext)
    : Repository<UserPreferencesEntity>(fitSyncDbContext), IUserPreferencesRepository
{

}
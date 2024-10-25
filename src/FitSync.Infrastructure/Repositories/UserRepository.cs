using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;

namespace FitSync.Infrastructure.Repositories;

public class UserRepository(FitSyncDbContext fitSyncDbContext)
    : Repository<UserEntity>(fitSyncDbContext), IUserRepository
{

}

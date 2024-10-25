using FitSync.Domain.Entities;

namespace FitSync.Domain.Interfaces;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity?> GetUserByIdIncludeAllAsync(int id);
}
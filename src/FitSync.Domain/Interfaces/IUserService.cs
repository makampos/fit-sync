using FitSync.Domain.Dtos;
using FitSync.Domain.Responses;

namespace FitSync.Domain.Interfaces;

public interface IUserService
{
    Task<ServiceResponse<int>> CreateUserAsync(UserDto user);
    Task<ServiceResponse<UserDto>> GetUserByIdAsync(int id);
}
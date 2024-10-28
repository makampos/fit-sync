using FitSync.Domain.Dtos;
using FitSync.Domain.Features.Users;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels;

namespace FitSync.Domain.Interfaces;

public interface IUserService
{
    Task<ServiceResponse<int>> CreateUserAsync(AddUser addUser);
    Task<ServiceResponse<UserDto>> GetUserByIdAsync(int id);
    Task<ServiceResponse<UserViewModel>> GetUserByIdIncludeAllAsync(int id);
}
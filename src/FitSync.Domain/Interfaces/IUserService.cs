using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels.Users;

namespace FitSync.Domain.Interfaces;

public interface IUserService
{
    Task<ServiceResponse<int>> CreateUserAsync(AddUserDto addUserDto);
    Task<ServiceResponse<UserViewModel>> GetUserByIdAsync(int id);
    Task<ServiceResponse<UserViewModelIncluded>> GetUserByIdIncludeAllAsync(int id);
}
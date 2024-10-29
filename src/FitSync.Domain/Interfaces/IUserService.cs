using FitSync.Domain.Features.Users;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels.Users;

namespace FitSync.Domain.Interfaces;

public interface IUserService
{
    Task<ServiceResponse<int>> CreateUserAsync(AddUser addUser);
    Task<ServiceResponse<UserViewModel>> GetUserByIdAsync(int id);
    Task<ServiceResponse<UserViewModelIncluded>> GetUserByIdIncludeAllAsync(int id);
}
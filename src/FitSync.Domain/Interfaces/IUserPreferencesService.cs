using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Dtos.Users.Preferences;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels.Users;

namespace FitSync.Domain.Interfaces;

public interface IUserPreferencesService
{
    Task<ServiceResponse<UserPreferencesViewModel>> GetUserPreferencesAsync(int userId);
    Task<ServiceResponse<int>> CreateUserPreferencesAsync(AddUserPreferencesDto addUserPreferencesDto);
    Task<ServiceResponse<bool>> UpdateUserPreferencesAsync(UpdateUserPreferencesDto updateUserPreferencesDto);
}
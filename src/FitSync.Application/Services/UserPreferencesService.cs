using FitSync.Application.Extensions;
using FitSync.Domain.Dtos.Users.Preferences;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels.Users;
using Microsoft.Extensions.Logging;

namespace FitSync.Application.Services;

public class UserPreferencesService : IUserPreferencesService
{
    private readonly ILogger<UserPreferencesService> _logger;
    private readonly IFitSyncUnitOfWork _fitSyncUnitOfWork;

    public UserPreferencesService(ILogger<UserPreferencesService> logger, IFitSyncUnitOfWork fitSyncUnitOfWork)
    {
        _logger = logger;
        _fitSyncUnitOfWork = fitSyncUnitOfWork;
    }

    public async Task<ServiceResponse<UserPreferencesViewModel>> GetUserPreferencesAsync(int userId)
    {
        _logger.LogInformation("Getting user preferences for user with id: {UserId}", userId);

        var userPreferencesEntity = await _fitSyncUnitOfWork.UserPreferencesRepository.GetByIdAsync(userId);

        if (userPreferencesEntity is null)
        {
            _logger.LogWarning("User preferences not found for user with id: {UserId}", userId);
            return ServiceResponse<UserPreferencesViewModel>.FailureResult("User preferences not found");
        }

        var userPreferencesViewModel = userPreferencesEntity.ToViewModel();
        return ServiceResponse<UserPreferencesViewModel>.SuccessResult(userPreferencesViewModel);
    }

    public async Task<ServiceResponse<int>> CreateUserPreferencesAsync(AddUserPreferencesDto addUserPreferencesDto)
    {
        //TODO: Get user id from claims
        _logger.LogInformation("Creating user preferences for user with id: {UserId}", "!claims!");

        var userPreferencesEntity = addUserPreferencesDto.ToDomainEntity();
        await _fitSyncUnitOfWork.UserPreferencesRepository.AddAsync(userPreferencesEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        return ServiceResponse<int>.SuccessResult(userPreferencesEntity.Id);
    }

    public async Task<ServiceResponse<bool>> UpdateUserPreferencesAsync(
        UpdateUserPreferencesDto updateUserPreferencesDto)
    {
        //TODO: Get user id from claims
        _logger.LogInformation("Updating user preferences for user with id: {UserId}", "!claims!");

        var userPreferencesEntity = await _fitSyncUnitOfWork.UserPreferencesRepository.GetByIdAsync(
            updateUserPreferencesDto.Id, CancellationToken.None);

        if (userPreferencesEntity is null)
        {
            _logger.LogWarning("User preferences not found with id: {UserPreferencesId}",
                updateUserPreferencesDto.Id);
            return ServiceResponse<bool>.FailureResult("User preferences not found");
        }

        userPreferencesEntity.Update(updateUserPreferencesDto.PreferredWeightUnit,
            updateUserPreferencesDto.PreferredDistanceUnit);

        await _fitSyncUnitOfWork.UserPreferencesRepository.UpdateAsync(userPreferencesEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        return ServiceResponse<bool>.SuccessResult(true);
    }
}
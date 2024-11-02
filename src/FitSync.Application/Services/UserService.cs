using FitSync.Application.Extensions;
using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels.Users;
using Microsoft.Extensions.Logging;

namespace FitSync.Application.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IFitSyncUnitOfWork _fitSyncUnitOfWork;
    public UserService(ILogger<UserService> logger, IFitSyncUnitOfWork fitSyncUnitOfWork)
    {
        _fitSyncUnitOfWork = fitSyncUnitOfWork;
        _logger = logger;
    }

    public async Task<ServiceResponse<int>> CreateUserAsync(AddUserDto addUserDto)
    {
        var userEntity = addUserDto.ToDomainEntity();

        await _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        _logger.LogInformation("User created with id: {Id}", userEntity.Id);

        return ServiceResponse<int>.SuccessResult(userEntity.Id);
    }

    public async Task<ServiceResponse<UserViewModel>> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("Getting user by id: {Id}", id);

        var userEntity = await _fitSyncUnitOfWork.UserRepository.GetByIdAsync(id, CancellationToken.None);

        if (userEntity is null)
        {
            _logger.LogWarning("User not found with id: {Id}", id);
            return ServiceResponse<UserViewModel>.FailureResult("User not found");
        }

        return ServiceResponse<UserViewModel>.SuccessResult(userEntity.ToViewModel());
    }

    public async Task<ServiceResponse<UserViewModelIncluded>> GetUserByIdIncludeAllAsync(int id)
    {
        _logger.LogInformation("Getting user by id: {Id} including all related data", id);

        var userEntity = await _fitSyncUnitOfWork.UserRepository.GetUserByIdIncludeAllAsync(id);

        if (userEntity is null)
        {
            _logger.LogWarning("User not found with id: {Id}", id);
            return ServiceResponse<UserViewModelIncluded>.FailureResult("User not found");
        }

        var userViewModelIncluded = userEntity.ToViewModelIncluded();

        return ServiceResponse<UserViewModelIncluded>.SuccessResult(userViewModelIncluded);
    }
}
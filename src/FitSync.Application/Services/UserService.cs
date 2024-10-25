using FitSync.Application.Mappers;
using FitSync.Domain.Dtos;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace FitSync.Application.Services;

public class UserService : IUserService
{
    private readonly IFitSyncUnitOfWork _fitSyncUnitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(IFitSyncUnitOfWork fitSyncUnitOfWork, ILogger<UserService> logger)
    {
        _fitSyncUnitOfWork = fitSyncUnitOfWork;
        _logger = logger;
    }

    public async Task<ServiceResponse<int>> CreateUserAsync(UserDto user)
    {
        var userEntity = user.ToDomainEntity();

        await _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        _logger.LogInformation("User created with id: {Id}", userEntity.Id);

        return ServiceResponse<int>.SuccessResult(userEntity.Id);
    }

    public async Task<ServiceResponse<UserDto>> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("Getting user by id: {Id}", id);

        var userEntity = await _fitSyncUnitOfWork.UserRepository.GetByIdAsync(id, CancellationToken.None);

        if (userEntity is null)
        {
            _logger.LogWarning("User not found with id: {Id}", id);
            return ServiceResponse<UserDto>.FailureResult("User not found");
        }

        return ServiceResponse<UserDto>.SuccessResult(userEntity.ToDto());
    }

    //TODO: Implement other methods
}
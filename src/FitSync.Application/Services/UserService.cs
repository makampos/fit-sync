using FitSync.Application.Mappers;
using FitSync.Domain.Dtos;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels;
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

    public async Task<ServiceResponse<UserViewModel>> GetUserByIdIncludeAllAsync(int id)
    {
        _logger.LogInformation("Getting user by id: {Id} including all related data", id);

        var userEntity = await _fitSyncUnitOfWork.UserRepository.GetUserByIdIncludeAllAsync(id);

        if (userEntity is null)
        {
            _logger.LogWarning("User not found with id: {Id}", id);
            return ServiceResponse<UserViewModel>.FailureResult("User not found");
        }

        //TODO: Refactor this logic to user Builder Pattern
        var workoutPlans = userEntity.WorkoutPlans
            .Select(wp => new WorkoutPlanViewModel(wp.Id, wp.Name, wp.Workouts
                .Select(w => w.Workout.ToViewModel())
                .ToList()))
            .ToList();

        var userViewModel = new UserViewModel(userEntity.Name, userEntity.Age, userEntity.Genre, workoutPlans);

        return ServiceResponse<UserViewModel>.SuccessResult(userViewModel);
    }

    //TODO: Implement other methods
}
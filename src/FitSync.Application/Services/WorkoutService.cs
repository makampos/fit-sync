using FitSync.Application.Mappers;
using FitSync.Domain.Dtos;
using FitSync.Domain.Enums;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Responses;
using FitSync.Domain.Results;
using Microsoft.Extensions.Logging;

namespace FitSync.Application.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IFitSyncUnitOfWork _fitSyncUnitOfWork;
    private readonly ILogger<WorkoutService> _logger;

    public WorkoutService(IFitSyncUnitOfWork fitSyncUnitOfWork, ILogger<WorkoutService> logger)
    {
        _fitSyncUnitOfWork = fitSyncUnitOfWork;
        _logger = logger;
    }

    public async Task<ServiceResponse<WorkoutDto>> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting workout by id: {Id}", id);

        var workoutEntity = await _fitSyncUnitOfWork.WorkoutRepository.GetByIdAsync(id, CancellationToken.None);

        if (workoutEntity is null)
        {
            _logger.LogWarning("Workout not found with id: {Id}", id);
            return ServiceResponse<WorkoutDto>.FailureResult("Workout not found");
        }

        return ServiceResponse<WorkoutDto>.SuccessResult(workoutEntity.ToDto());
    }

    public async Task<ServiceResponse<PagedResult<WorkoutDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("Getting all workouts");

        var pagedResult = await _fitSyncUnitOfWork.WorkoutRepository.GetAllAsync(pageNumber, pageSize,
            CancellationToken.None);

        return ServiceResponse<PagedResult<WorkoutDto>>.SuccessResult(pagedResult.ToDto(x => x.ToDto()));
    }

    public async Task<ServiceResponse<PagedResult<WorkoutDto>>> GetFilteredWorkoutsAsync(
        WorkoutType? type = null, string? bodyPart = null, string? equipment = null, WorkoutLevel? level = null,
        int pageNumber = 1, int pageSize = 10)
    {
        _logger.LogInformation("Getting filtered workouts");

        var pagedResult = await _fitSyncUnitOfWork.WorkoutRepository.GetFilteredWorkoutsAsync(
            type,
            bodyPart,
            equipment,
            level,
            pageNumber,
            pageSize,
            CancellationToken.None);

        return ServiceResponse<PagedResult<WorkoutDto>>.SuccessResult(pagedResult.ToDto(x => x.ToDto()));
    }

    public async Task<ServiceResponse<int>> CreateAsync(WorkoutDto workout)
    {
        _logger.LogInformation("Creating workout: {Workout}", workout);

        var workoutEntity = workout.ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("Workout created with id: {Id}", workoutEntity.Id);

        return ServiceResponse<int>.SuccessResult(workoutEntity.Id);
    }

    public async Task<ServiceResponse<bool>> UpdateAsync(WorkoutDto workoutDto)
    {
        _logger.LogInformation("Updating workout: {Workout}", workoutDto);

        var workoutEntity = await _fitSyncUnitOfWork.WorkoutRepository.GetByIdAsync(workoutDto.Id, CancellationToken.None);

        if (workoutEntity is null)
        {
            _logger.LogWarning("Workout not found with id: {Id}", workoutDto.Id);
            return ServiceResponse<bool>.FailureResult("Workout not found");
        }

        await _fitSyncUnitOfWork.WorkoutRepository.UpdateAsync(workoutEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("Workout updated with id: {Id}", workoutEntity.Id);

        return ServiceResponse<bool>.SuccessResult(true);
    }

    public async Task<ServiceResponse<bool>> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting workout by id: {Id}", id);

        var workoutEntity = await _fitSyncUnitOfWork.WorkoutRepository.GetByIdAsync(id, CancellationToken.None);

        if (workoutEntity is null)
        {
            _logger.LogWarning("Workout not found with id: {Id}", id);

            return ServiceResponse<bool>.FailureResult("Workout not found");
        }

        await _fitSyncUnitOfWork.WorkoutRepository.DeleteAsync(workoutEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("Workout deleted with id: {Id}", id);

        return ServiceResponse<bool>.SuccessResult(true);
    }
}
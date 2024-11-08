using FitSync.Application.Extensions;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels.WorkoutPlans;
using Microsoft.Extensions.Logging;

namespace FitSync.Application.Services;

public class WorkoutPlanService : IWorkoutPlanService
{

    private readonly ILogger<WorkoutPlanService> _logger;
    private readonly IFitSyncUnitOfWork _fitSyncUnitOfWork;

    public WorkoutPlanService(ILogger<WorkoutPlanService> logger, IFitSyncUnitOfWork fitSyncUnitOfWork)
    {
        _logger = logger;
        _fitSyncUnitOfWork = fitSyncUnitOfWork;
    }

    public async Task<ServiceResponse<WorkoutPlanViewModel>> GetWorkoutPlanByIdAsync(int id)
    {
        //TODO: Revisited this to check if is right to filter (First)
        _logger.LogInformation("Getting workout plan by id: {id}", id);

        var workoutPlanWorkoutsEntity =
            await _fitSyncUnitOfWork.WorkoutPlanWorkoutRepository.GetWorkoutPlanWorkoutsByWorkoutPlanIdAsync(id);

        if (!workoutPlanWorkoutsEntity.Any())
        {
            return ServiceResponse<WorkoutPlanViewModel>.FailureResult($"Workout plan with id {id} not found");
        }

        var workoutPlanId = workoutPlanWorkoutsEntity.First().WorkoutPlanId;
        var workoutPlanName = workoutPlanWorkoutsEntity.First().WorkoutPlan.Name;
        var isWorkoutPlanActive = workoutPlanWorkoutsEntity.First().WorkoutPlan.IsActive;

        var workoutPlanViewModel = WorkoutPlanViewModel.Create(
            workoutPlanId: workoutPlanId,
            name: workoutPlanName,
            isActive: isWorkoutPlanActive,
            workoutWithExercisesSetViewModel: workoutPlanWorkoutsEntity
                .Select(w => w.Workout.ToViewModel(ExerciseSet.Create(w.Sets, w.RepsMin, w.RepsMax,
                    w.Weight, w.RestBetweenSets, w.Notes)))
                .ToList());

        return ServiceResponse<WorkoutPlanViewModel>.SuccessResult(workoutPlanViewModel);
    }

    public async Task<ServiceResponse<IEnumerable<WorkoutPlanViewModel>>> GetWorkoutPlansByUserIdAsync(int userId)
    {
        _logger.LogInformation("Getting workout plan by user id: {userId}", userId);

        var workoutPlanEntity = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetWorkoutPlanIncludedWorkoutsByUserIdAsync(userId);

        if (!workoutPlanEntity.Any())
        {
            return ServiceResponse<IEnumerable<WorkoutPlanViewModel>>.FailureResult($"Workout plan with user id {userId} not found");
        }

        var workoutPlanViewModels = workoutPlanEntity
            .Select(x => WorkoutPlanViewModel.Create(
                workoutPlanId: x.Id,
                name: x.Name,
                isActive: x.IsActive,
                workoutWithExercisesSetViewModel: x.WorkoutPlanWorkoutEntities
                    .Select(w =>
                        w.Workout.ToViewModel(ExerciseSet.Create(w.Sets, w.RepsMin, w.RepsMax, w.Weight, w.RestBetweenSets,
                            w.Notes)))
                    .ToList()));

        return ServiceResponse<IEnumerable<WorkoutPlanViewModel>>.SuccessResult(workoutPlanViewModels);
    }

    public async Task<ServiceResponse<int>> CreateWorkPlanAsync(AddWorkoutPlanDto addWorkoutPlanDto)
    {
        _logger.LogInformation("Creating new workout plan");

        var workPlanEntity = addWorkoutPlanDto.ToDomainEntity();
        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        return ServiceResponse<int>.SuccessResult(workPlanEntity.Id);
    }

    public async Task<ServiceResponse<bool>> UpdateWorkPlanAsync(UpdateWorkoutPlanDto updateWorkoutPlanDto)
    {
        _logger.LogInformation("Updating workout plan");

        var workoutPlanWorkoutEntities = await _fitSyncUnitOfWork.WorkoutPlanWorkoutRepository
            .GetWorkoutPlanWorkoutsByWorkoutPlanIdAsync(updateWorkoutPlanDto.Id);

        if (!workoutPlanWorkoutEntities.Any())
        {
            _logger.LogWarning("Workout plan with id {id} not found", updateWorkoutPlanDto.Id);
            return ServiceResponse<bool>.FailureResult("Workout plan not found");
        }

        foreach (var item in workoutPlanWorkoutEntities)
        {
            item.Update(item.WorkoutId,
                updateWorkoutPlanDto.Workouts[item.WorkoutId].Sets,
                updateWorkoutPlanDto.Workouts[item.WorkoutId].RepsMin,
                updateWorkoutPlanDto.Workouts[item.WorkoutId].RepsMax,
                updateWorkoutPlanDto.Workouts[item.WorkoutId].Weight,
                updateWorkoutPlanDto.Workouts[item.WorkoutId].RestBetweenSets,
                updateWorkoutPlanDto.Workouts[item.WorkoutId].Notes);
        }

        var workoutPlanEntity = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetByIdAsync(updateWorkoutPlanDto.Id);
        workoutPlanEntity!.Update(updateWorkoutPlanDto.Name);

        await Task.WhenAll(
            _fitSyncUnitOfWork.WorkoutPlanRepository.UpdateAsync(workoutPlanEntity),
            _fitSyncUnitOfWork.WorkoutPlanWorkoutRepository.UpdateRangeAsync(workoutPlanWorkoutEntities));

        await _fitSyncUnitOfWork.SaveChangesAsync();

        return ServiceResponse<bool>.SuccessResult(true);
    }

    public async Task<ServiceResponse<bool>> ToggleWorkoutPlanActiveAsync(UpdateWorkoutPlanActiveOrInactiveDto
        updateWorkoutPlanActiveOrInactiveDto)
    {
        _logger.LogInformation("Activating or deactivating workout plan with id {WorkoutPlanId}",
            updateWorkoutPlanActiveOrInactiveDto.WorkoutPlanId);

        var workoutPlanEntity = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetByIdAsync
            (updateWorkoutPlanActiveOrInactiveDto.WorkoutPlanId);

        if (workoutPlanEntity is null)
        {
            _logger.LogWarning("Workout plan with id {id} not found", updateWorkoutPlanActiveOrInactiveDto.WorkoutPlanId);
            return ServiceResponse<bool>.FailureResult("Workout plan not found");
        }

        if (workoutPlanEntity.IsActive == updateWorkoutPlanActiveOrInactiveDto.IsActive)
        {
           _logger.LogWarning("Can not update workout plan status, because it is already {IsActive}",
               workoutPlanEntity.IsActive);
           return ServiceResponse<bool>.FailureResult("Can not update workout plan status");
        }

        workoutPlanEntity.ToggleIsActive(updateWorkoutPlanActiveOrInactiveDto.IsActive);
        await _fitSyncUnitOfWork.WorkoutPlanRepository.UpdateAsync(workoutPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        return ServiceResponse<bool>.SuccessResult(true);
    }

    public async Task<ServiceResponse<bool>> DeleteWorkPlanAsync(int id)
    {
        _logger.LogInformation("Deleting workout plan");

        var workoutPlanEntity = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetByIdAsync(id);

        if (workoutPlanEntity is null)
        {
            _logger.LogWarning("Workout plan with id {id} not found", id);
            return ServiceResponse<bool>.FailureResult("Workout plan not found");
        }

        var workoutPlanWorkoutEntities = await _fitSyncUnitOfWork.WorkoutPlanWorkoutRepository
            .GetWorkoutPlanWorkoutsByWorkoutPlanIdAsync(workoutPlanEntity.Id);


        await Task.WhenAll(
            _fitSyncUnitOfWork.WorkoutPlanRepository.DeleteAsync(workoutPlanEntity),
            _fitSyncUnitOfWork.WorkoutPlanWorkoutRepository.DeleteRangeAsync(workoutPlanWorkoutEntities));

        await _fitSyncUnitOfWork.SaveChangesAsync();

        return ServiceResponse<bool>.SuccessResult(true);
    }
}
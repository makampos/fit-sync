using FitSync.Application.Mappers;
using FitSync.Domain.Dtos;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels;
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

    public async Task<ServiceResponse<int>> CreateWorkPlanAsync(AddWorkoutPlanDto addWorkoutPlanDto)
    {
        _logger.LogInformation("Creating new workout plan");

        var workPlanEntity = addWorkoutPlanDto.ToDomainEntity();
        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        return ServiceResponse<int>.SuccessResult(workPlanEntity.Id);
    }

    public async Task<ServiceResponse<WorkoutPlanViewModel>> GetWorkoutPlansByIdAsync(int id)
    {
        _logger.LogInformation("Getting workout plan by id: {id}", id);

        var workoutPlanEntity = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetWorkoutPlanIncludedWorkoutsAsync(id);

        if (workoutPlanEntity is null)
        {
            return ServiceResponse<WorkoutPlanViewModel>.FailureResult($"Workout plan with id {id} not found");
        }

        var workoutEntities =  await _fitSyncUnitOfWork.WorkoutRepository
            .GetWorkoutsByIdsAsync(workoutPlanEntity.Workouts.Select(x => x.WorkoutId));

        var workoutViewModel =
            new WorkoutPlanViewModel(workoutPlanEntity.Id, workoutPlanEntity.Name,
                workoutEntities.Select(x => x.ToViewModel()).ToList());

        return ServiceResponse<WorkoutPlanViewModel>.SuccessResult(workoutViewModel);
    }


    // TODO: Implement proper view model for workout plan
    public async Task<ServiceResponse<IEnumerable<WorkoutPlanViewModel>>> GetWorkoutPlansByUserIdAsync(int userId)
    {
        _logger.LogInformation("Getting workout plan by user id: {userId}", userId);

        var workoutPlanEntity = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetWorkoutPlanIncludedWorkoutsByUserIdAsync(userId);

        if (!workoutPlanEntity.Any())
        {
            return ServiceResponse<IEnumerable<WorkoutPlanViewModel>>.FailureResult($"Workout plan with user id {userId} not found");
        }

        var workoutPlanViewModels = workoutPlanEntity
            .Select(x => new WorkoutPlanViewModel(x.Id, x.Name, x.Workouts
                .Select(w => w.Workout.ToViewModel())
                .ToList()));

        return ServiceResponse<IEnumerable<WorkoutPlanViewModel>>.SuccessResult(workoutPlanViewModels);
    }
}
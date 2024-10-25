using FitSync.Application.Mappers;
using FitSync.Domain.Dtos;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Responses;
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

    public async Task<ServiceResponse<WorkoutPlanViewModel>> GetWorkPlanByIdAsync(int id)
    {
        _logger.LogInformation("Getting workout plan by id: {id}", id);

        var workPlanEntity = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetWorkoutPlanWithWorkoutsAsync(id);

        if (workPlanEntity is null)
        {
            return ServiceResponse<WorkoutPlanViewModel>.FailureResult($"Workout plan with id {id} not found");
        }

        var workoutEntities =  await _fitSyncUnitOfWork.WorkoutRepository.GetWorkoutsByIdsAsync(workPlanEntity.Workouts.Select(x => x.WorkoutId));


        var workoutViewModel =
            new WorkoutPlanViewModel(workPlanEntity.Id, workPlanEntity.Name, workoutEntities.Select(x => x.ToDto()).ToList());

        return ServiceResponse<WorkoutPlanViewModel>.SuccessResult(workoutViewModel);
    }
}
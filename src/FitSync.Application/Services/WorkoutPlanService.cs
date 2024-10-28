using FitSync.Application.Mappers;
using FitSync.Domain.Features.WorkoutPlans;
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

    public async Task<ServiceResponse<int>> CreateWorkPlanAsync(AddWorkoutPlan addWorkoutPlan)
    {
        _logger.LogInformation("Creating new workout plan");

        var workPlanEntity = addWorkoutPlan.ToDomainEntity();
        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        return ServiceResponse<int>.SuccessResult(workPlanEntity.Id);
    }

    public async Task<ServiceResponse<WorkoutPlanViewModel>> GetWorkoutPlansByIdAsync(int id)
    {
        _logger.LogInformation("Getting workout plan by id: {id}", id);

        var workoutPlanWorkoutsEntity =
            await _fitSyncUnitOfWork.WorkoutPlanWorkoutRepository.GetWorkoutPlanWorkoutsByWorkoutPlanIdAsync(id);

        if (!workoutPlanWorkoutsEntity.Any())
        {
            return ServiceResponse<WorkoutPlanViewModel>.FailureResult($"Workout plan with id {id} not found");
        }

        var workoutPlanId = workoutPlanWorkoutsEntity.First().WorkoutPlanId;
        var workoutPlanName = workoutPlanWorkoutsEntity.First().WorkoutPlan.Name;

        var workoutPlanViewModel = WorkoutPlanViewModel.Create(
            id: workoutPlanId,
            name: workoutPlanName,
            workoutsViewModel: workoutPlanWorkoutsEntity
                .Select(w => w.Workout.ToViewModel(ExerciseSet.Create(w.Sets, w.RepsMin, w.RepsMax, w.RestBetweenSets, w.Notes)))
                .ToList());

        return ServiceResponse<WorkoutPlanViewModel>.SuccessResult(workoutPlanViewModel);
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
            .Select(x => WorkoutPlanViewModel.Create(
                id: x.Id,
                name: x.Name,
                workoutsViewModel: x.WorkoutPlanWorkoutEntities
                    .Select(w =>
                        w.Workout.ToViewModel(ExerciseSet.Create(w.Sets, w.RepsMin, w.RepsMax, w.RestBetweenSets,
                            w.Notes)))
                    .ToList()));

        return ServiceResponse<IEnumerable<WorkoutPlanViewModel>>.SuccessResult(workoutPlanViewModels);
    }
}
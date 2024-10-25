using FitSync.Domain.Dtos;
using FitSync.Domain.Responses;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutPlanService
{
    Task<ServiceResponse<int>> CreateWorkPlanAsync(AddWorkoutPlanDto workoutPlanDto);
    Task<ServiceResponse<WorkoutPlanViewModel>> GetWorkPlanByIdAsync(int id);
}
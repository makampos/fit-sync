using FitSync.Domain.Dtos;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutPlanService
{
    Task<ServiceResponse<int>> CreateWorkPlanAsync(AddWorkoutPlanDto workoutPlanDto);
    Task<ServiceResponse<WorkoutPlanViewModel>> GetWorkoutPlansByIdAsync(int id);
    Task<ServiceResponse<IEnumerable<WorkoutPlanViewModel>>> GetWorkoutPlansByUserIdAsync(int userId);
}
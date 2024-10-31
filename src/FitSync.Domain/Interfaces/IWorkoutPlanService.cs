using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Responses;
using FitSync.Domain.ViewModels.WorkoutPlans;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutPlanService
{
    Task<ServiceResponse<int>> CreateWorkPlanAsync(AddWorkoutPlanDto addWorkoutPlanDto);
    Task<ServiceResponse<bool>> UpdateWorkPlanAsync(UpdateWorkoutPlanDto updateWorkoutPlanDto);
    Task<ServiceResponse<bool>> DeleteWorkPlanAsync(int id);
    Task<ServiceResponse<WorkoutPlanViewModel>> GetWorkoutPlanByIdAsync(int id);
    Task<ServiceResponse<IEnumerable<WorkoutPlanViewModel>>> GetWorkoutPlansByUserIdAsync(int userId);
}
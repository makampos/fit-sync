using FitSync.Domain.Dtos.Workouts;
using FitSync.Domain.Enums;
using FitSync.Domain.Responses;
using FitSync.Domain.Results;
using FitSync.Domain.ViewModels.Workouts;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutService
{
    Task<ServiceResponse<WorkoutViewModel>> GetByIdAsync(int id);
    Task<ServiceResponse<PagedResult<WorkoutViewModel>>> GetAllAsync(int pageNumber, int pageSize);
    Task<ServiceResponse<PagedResult<WorkoutViewModel>>> GetFilteredWorkoutsAsync(
        WorkoutType? type = null,
        string? bodyPart = null,
        string? equipment = null,
        WorkoutLevel? level = null,
        int pageNumber = 1,
        int pageSize = 10);
    Task<ServiceResponse<int>> CreateAsync(AddWorkoutDto addWorkoutDto);
    Task<ServiceResponse<bool>> UpdateAsync(UpdateWorkoutDto updateWorkoutDto);
    Task<ServiceResponse<bool>> DeleteAsync(int id);
}
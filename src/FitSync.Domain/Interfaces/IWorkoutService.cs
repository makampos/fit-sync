using FitSync.Domain.Dtos;
using FitSync.Domain.Entities;
using FitSync.Domain.Enums;
using FitSync.Domain.Responses;
using FitSync.Domain.Results;

namespace FitSync.Domain.Interfaces;

public interface IWorkoutService
{
    Task<ServiceResponse<WorkoutDto>> GetByIdAsync(int id);
    Task<ServiceResponse<PagedResult<WorkoutDto>>> GetAllAsync(int pageNumber, int pageSize);
    Task<ServiceResponse<PagedResult<WorkoutDto>>> GetFilteredWorkoutsAsync(
        WorkoutType? type = null,
        string? bodyPart = null,
        string? equipment = null,
        WorkoutLevel? level = null,
        int pageNumber = 1,
        int pageSize = 10);
    Task<ServiceResponse<int>> CreateAsync(WorkoutDto workoutDto);
    Task<ServiceResponse<bool>> UpdateAsync(WorkoutDto workoutDto);
    Task<ServiceResponse<bool>> DeleteAsync(int id);
}
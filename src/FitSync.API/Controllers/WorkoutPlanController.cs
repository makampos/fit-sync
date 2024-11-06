using FitSync.API.Responses;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Interfaces;
using FitSync.Domain.ViewModels.WorkoutPlans;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;

[ApiController]
[Route("api/workout-plans")]
public class WorkoutPlanController : ControllerBase
{
    private readonly ILogger<WorkoutPlanController> _logger;
    private readonly IWorkoutPlanService _workoutPlanService;

    public WorkoutPlanController(ILogger<WorkoutPlanController> logger, IWorkoutPlanService workoutPlanService)
    {
        _logger = logger;
        _workoutPlanService = workoutPlanService;
    }

    [HttpGet("user/{userId}")]
    [SwaggerOperation("Get Workout Plan by User Id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Workout plan found", typeof(IEnumerable<WorkoutPlanViewModel>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Workout plan not found")]
    public async Task<IActionResult> GetWorkoutPlansByUserIdAsync([FromRoute] int userId)
    {
        _logger.LogInformation("Getting workout plan by user id: {userId}", userId);
        var serviceResponse = await _workoutPlanService.GetWorkoutPlansByUserIdAsync(userId);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound();
    }

    [HttpGet("{id}", Name = nameof(GetWorkoutPlanByIdAsync))]
    [SwaggerOperation("Get Workout Plan by Id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Workout plan found", typeof(WorkoutPlanViewModel))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Workout plan not found")]
    public async Task<IActionResult> GetWorkoutPlanByIdAsync(int id)
    {
        _logger.LogInformation("Getting workout plan by id: {id}", id);
        var serviceResponse = await _workoutPlanService.GetWorkoutPlanByIdAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound();
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Workout plan created", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid workout plan data")]
    public async Task<IActionResult> CreateWorkoutPlanAsync([FromBody] AddWorkoutPlanDto addWorkoutPlanDto)
    {
        _logger.LogInformation("Creating new workout plan");

        var serviceResponse = await _workoutPlanService.CreateWorkPlanAsync(addWorkoutPlanDto);

        var resource = Resource.Create(serviceResponse.Data);

        return serviceResponse.Success
            ? CreatedAtRoute(nameof(GetWorkoutPlanByIdAsync), resource.GetRouteValues(), resource.GetCreatedResource())
            : BadRequest(serviceResponse.ErrorMessage);
    }

    [HttpPut]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Workout plan updated")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid workout plan data")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Workout plan not found")]
    public async Task<IActionResult> UpdateWorkoutPlanAsync([FromBody] UpdateWorkoutPlanDto updateWorkoutPlanDto)
    {
        _logger.LogInformation("Updating workout plan");

        var serviceResponse = await _workoutPlanService.UpdateWorkPlanAsync(updateWorkoutPlanDto);

        return serviceResponse.Success
            ? NoContent()
            : BadRequest(serviceResponse.ErrorMessage);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete Workout Plan")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Workout plan deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Workout plan not found")]
    public async Task<IActionResult> DeleteWorkoutPlanAsync(int id)
    {
        _logger.LogInformation("Deleting workout plan by id: {id}", id);
        var serviceResponse = await _workoutPlanService.DeleteWorkPlanAsync(id);

        return serviceResponse.Success
            ? NoContent()
            : NotFound();
    }
}
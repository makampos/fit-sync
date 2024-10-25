using FitSync.Domain.Dtos;
using FitSync.Domain.Interfaces;
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

    [HttpGet("{id}", Name = nameof(GetWorkoutPlanByIdAsync))]
    [SwaggerOperation("Get Workout Plan by Id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Workout plan found", typeof(WorkoutPlanDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Workout plan not found")]
    public async Task<IActionResult> GetWorkoutPlanByIdAsync(int id)
    {
        _logger.LogInformation("Getting workout plan by id: {id}", id);
        var serviceResponse = await _workoutPlanService.GetWorkPlanByIdAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound();
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Workout plan created", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid workout plan data")]
    public async Task<IActionResult> CreateWorkoutPlanAsync([FromBody] AddWorkoutPlanDto workoutPlanDto)
    {
        _logger.LogInformation("Creating new workout plan");

        var serviceResponse = await _workoutPlanService.CreateWorkPlanAsync(workoutPlanDto);

        var createdResource = new { Id = serviceResponse.Data, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return serviceResponse.Success
            ? CreatedAtRoute(nameof(GetWorkoutPlanByIdAsync), routeValues, createdResource)
            : BadRequest(serviceResponse.ErrorMessage);
    }
}
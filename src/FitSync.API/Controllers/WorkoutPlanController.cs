using FitSync.API.Responses;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Interfaces;
using FitSync.Domain.ViewModels.WorkoutPlans;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;

[SwaggerTag("Endpoints for workout plan related operations")]
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

    [HttpGet("users/{userId:int}")]
    [SwaggerOperation("Get resource by user id and flag")]
    [SwaggerResponse(StatusCodes.Status200OK, "The resource is returned", typeof(IEnumerable<WorkoutPlanViewModel>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The resource is not found")]
    public async Task<IActionResult> GetWorkoutPlansByUserIdAsync([FromRoute] int userId, [FromQuery] bool isActive =
        false)
    {
        // TODO: add pagination support
        _logger.LogInformation("Getting workout plan by user id: {userId}", userId);
        var serviceResponse = await _workoutPlanService.GetWorkoutPlansByUserIdAsync(userId, isActive);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound();
    }


    [HttpGet("{id:int}", Name = nameof(GetWorkoutPlanByIdAsync))]
    [SwaggerOperation("Get resource by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "The resource is returned", typeof(WorkoutPlanViewModel))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The resource is not found")]
    public async Task<IActionResult> GetWorkoutPlanByIdAsync([FromRoute] int id)
    {
        _logger.LogInformation("Getting workout plan by id: {id}", id);
        var serviceResponse = await _workoutPlanService.GetWorkoutPlanByIdAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound();
    }

    [HttpPost]
    [SwaggerOperation("Create a new resource")]
    [SwaggerResponse(StatusCodes.Status201Created, "A new resource is created", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error is returned")]
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
    [SwaggerOperation("Update resource")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The resource is updated")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error is returned")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The resource is not found")]
    public async Task<IActionResult> UpdateWorkoutPlanAsync([FromBody] UpdateWorkoutPlanDto updateWorkoutPlanDto)
    {
        _logger.LogInformation("Updating workout plan");

        var serviceResponse = await _workoutPlanService.UpdateWorkPlanAsync(updateWorkoutPlanDto);

        return serviceResponse.Success
            ? NoContent()
            : serviceResponse.ErrorMessage switch
            {
                "Workout plan not found" => NotFound(),
                _ => BadRequest()
            };
    }

    [HttpPatch("{id:int}/toggle-active")]
    [SwaggerOperation("Toggle resource flag")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The resource is updated")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error is returned")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The resource is not found")]
    public async Task<IActionResult> ToggleWorkoutPlanActiveAsync([FromRoute] int id, [FromBody] bool
        isActive)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(WorkoutPlanController),
            nameof(ToggleWorkoutPlanActiveAsync));

        var serviceResponse =
            await _workoutPlanService.ToggleWorkoutPlanActiveAsync(UpdateWorkoutPlanActiveOrInactiveDto
            .Create(id, isActive));

        return serviceResponse.Success
            ? NoContent()
            : serviceResponse.ErrorMessage switch
            {
                "Workout plan not found" => NotFound(),
                _ => BadRequest(serviceResponse.ErrorMessage)
            };
    }


    [HttpDelete("{id:int}")]
    [SwaggerOperation("Delete resource")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The resource is deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The resource is not found")]
    public async Task<IActionResult> DeleteWorkoutPlanAsync([FromRoute] int id)
    {
        _logger.LogInformation("Deleting workout plan by id: {id}", id);
        var serviceResponse = await _workoutPlanService.DeleteWorkPlanAsync(id);

        return serviceResponse.Success
            ? NoContent()
            : NotFound();
    }
}
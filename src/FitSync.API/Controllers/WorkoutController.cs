using FitSync.Domain.Dtos;
using FitSync.Domain.Enums;
using FitSync.Domain.Interfaces;
using FitSync.Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;

[ApiController]
[Route("/api/workouts")]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _workoutService;
    private readonly ILogger<WorkoutController> _logger;

    public WorkoutController(IWorkoutService workoutService,
        ILogger<WorkoutController> logger)
    {
        _workoutService = workoutService;
        _logger = logger;
    }

    [HttpGet("{id}", Name = nameof(GetByIdAsync))]
    [SwaggerOperation("Get Workout by Id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Workout found", typeof(WorkoutDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Workout not found")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(WorkoutController), nameof(GetByIdAsync));
        var serviceResponse = await _workoutService.GetByIdAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound();
    }

    [HttpGet]
    [SwaggerOperation("Get All Workouts")]
    [SwaggerResponse(StatusCodes.Status200OK, "Workouts", typeof(PagedResult<WorkoutDto>))]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] WorkoutType? type = null,
        [FromQuery] string? bodyPart = null,
        [FromQuery] string? equipment = null,
        [FromQuery] WorkoutLevel? level = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(WorkoutController),
            nameof(GetAllAsync));
        var serviceResponse = await _workoutService.GetFilteredWorkoutsAsync(type, bodyPart, equipment, level,
            pageNumber, pageSize);

        return Ok(serviceResponse.Data);
    }

    [HttpPost]
    [SwaggerOperation("Create Workout")]
    [SwaggerResponse(StatusCodes.Status201Created, "Workout created", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Input")]
    public async Task<IActionResult> CreateAsync(WorkoutDto workoutDto)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(WorkoutController), nameof(CreateAsync));
        var serviceResponse = await _workoutService.CreateAsync(workoutDto);

        return CreatedAtRoute(nameof(GetByIdAsync), new { serviceResponse.Data }, serviceResponse.Data);
    }

    [HttpPut]
    [SwaggerOperation("Update Workout")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Workout updated")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Input")]
    public async Task<IActionResult> UpdateAsync(WorkoutDto workout)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(WorkoutController), nameof(UpdateAsync));
        var serviceResponse = await _workoutService.UpdateAsync(workout);

        return serviceResponse.Success
            ? NoContent()
            : serviceResponse.ErrorMessage switch
            {
                "Workout not found" => NotFound(),
                _ => BadRequest()
            };
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete Workout")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Workout deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Workout not found")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(WorkoutController), nameof(DeleteAsync));
        var serviceResponse = await _workoutService.DeleteAsync(id);

        return serviceResponse.Success
            ? NoContent()
            : NotFound();
    }
}
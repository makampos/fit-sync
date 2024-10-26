using FitSync.Domain.Interfaces;
using FitSync.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;

[ApiController]
[Route("api/calendar")]
public class CalendarController : ControllerBase
{
    private readonly ILogger<CalendarController> _logger;
    private readonly ICalendarService _calendarService;

    public CalendarController(ILogger<CalendarController> logger, ICalendarService calendarService)
    {
        _logger = logger;
        _calendarService = calendarService;
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, "Event added to calendar")]
    public async Task<IActionResult> AddEventAsync([FromBody] ICollection<WorkoutPlanViewModel> workoutPlansViewModel)
    {
        _logger.LogInformation("Adding new event to calendar");
        var result = await _calendarService.AddEventAsync(workoutPlansViewModel);
        return Ok(result);
    }
}
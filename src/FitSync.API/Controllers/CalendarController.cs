using System.Text;
using FitSync.Domain.Dtos.CalendarEvents;
using FitSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;


[ApiController]
[Route("api/calendar")]
[SwaggerTag("Endpoints for calendar related operations")]
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
    [SwaggerOperation("Export .ICS file")]
    [SwaggerResponse(StatusCodes.Status200OK, "Add workout plans to calendar events and returned a .ICS file", typeof(FileContentHttpResult))]
    public async Task<IActionResult> AddEventAsync(
        [FromBody] AddWorkoutPlanCalendarEventDto addWorkoutPlanCalendarEventsDto)
    {
        _logger.LogInformation("Adding new event to calendar");
        var content = await _calendarService.AddEventAsync(addWorkoutPlanCalendarEventsDto);

        var fileBytes = Encoding.UTF8.GetBytes(content);

        var file = File(fileBytes, "text/calendar", $"{nameof(addWorkoutPlanCalendarEventsDto)}.ics");

        return file;
    }
}
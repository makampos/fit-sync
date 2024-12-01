using FitSync.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;

[SwaggerTag("Endpoints for data import related operations")]
[ApiController]
[Route("/api/dataimports")]
public class DataImportController : ControllerBase
{
    private readonly IDataImportService _dataImportService;

    public DataImportController(IDataImportService dataImportService)
    {
        _dataImportService = dataImportService;
    }

    [HttpPost]
    [SwaggerOperation("Import Workouts data from CSV file")]
    [SwaggerResponse(StatusCodes.Status201Created)]
    public async Task<IActionResult> ImportData(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        await _dataImportService.ImportWorkoutsAsync(memoryStream);

        return Created();
    }
}
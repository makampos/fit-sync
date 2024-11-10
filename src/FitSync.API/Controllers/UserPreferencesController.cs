using FitSync.API.Responses;
using FitSync.Domain.Dtos.Users.Preferences;
using FitSync.Domain.Interfaces;
using FitSync.Domain.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;

[ApiController]
[Route("api/user-preferences")]
public class UserPreferencesController : ControllerBase
{
    private ILogger<UserPreferencesController> _logger;
    private IUserPreferencesService _userPreferencesService;

    public UserPreferencesController(ILogger<UserPreferencesController> logger, IUserPreferencesService userPreferencesService)
    {
        _logger = logger;
        _userPreferencesService = userPreferencesService;
    }

    [HttpGet("{id:int}", Name = nameof(GetUserPreferencesByIdAsync))]
    [SwaggerResponse(StatusCodes.Status200OK, "User preferences found", typeof(UserPreferencesViewModel))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User preferences not found")]
    public async Task<IActionResult> GetUserPreferencesByIdAsync([FromRoute] int id)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserPreferencesController), nameof(GetUserPreferencesByIdAsync));
        var serviceResponse = await _userPreferencesService.GetUserPreferencesAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound(serviceResponse.ErrorMessage);
    }

    [HttpPost(Name = nameof(CreateUserPreferencesAsync))]
    [SwaggerResponse(StatusCodes.Status201Created, "User preferences created", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "User preferences data is invalid")]
    public async Task<IActionResult> CreateUserPreferencesAsync([FromBody] AddUserPreferencesDto addUserPreferencesDto)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserPreferencesController), nameof(CreateUserPreferencesAsync));
        var serviceResponse = await _userPreferencesService.CreateUserPreferencesAsync(addUserPreferencesDto);

        var resource = Resource.Create(serviceResponse.Data);

        return serviceResponse.Success
            ? CreatedAtRoute(nameof(GetUserPreferencesByIdAsync), resource.GetRouteValues(), resource.GetCreatedResource())
            : BadRequest(serviceResponse.ErrorMessage);
    }

    [HttpPut("{id:int}", Name = nameof(UpdateUserPreferencesAsync))]
    [SwaggerOperation("Update User Preferences")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User Preferences Updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User Preferences Not Found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Input")]
    public async Task<IActionResult> UpdateUserPreferencesAsync(
        [FromRoute] int id,
        [FromBody] UpdateUserPreferencesDto updateUserPreferencesDto)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserPreferencesController), nameof(UpdateUserPreferencesAsync));
        var serviceResponse = await _userPreferencesService.UpdateUserPreferencesAsync(updateUserPreferencesDto
            .WithId(id));

        return serviceResponse.Success
            ? NoContent()
            : serviceResponse.ErrorMessage switch
            {
                "User preferences not found" => NotFound(),
                _ => BadRequest()
            };
    }
}
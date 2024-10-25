using FitSync.Domain.Dtos;
using FitSync.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet("{id}",Name = nameof(GetUserByIdAsync))]
    [SwaggerResponse(StatusCodes.Status200OK, "User found", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserController), nameof(GetUserByIdAsync));
        var serviceResponse = await _userService.GetUserByIdAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound(serviceResponse.ErrorMessage);
    }

    [HttpGet("{id}/include-all", Name = nameof(GetUserByIdIncludeAllAsync))]
    [SwaggerResponse(StatusCodes.Status200OK, "User found", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> GetUserByIdIncludeAllAsync(int id)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserController), nameof(GetUserByIdIncludeAllAsync));
        var serviceResponse = await _userService.GetUserByIdIncludeAllAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound(serviceResponse.ErrorMessage);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "User created", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "User data is invalid")]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto user)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserController), nameof(CreateUserAsync));
        var serviceResponse = await _userService.CreateUserAsync(user);

        var createdResource = new { Id = serviceResponse.Data, Version = "1.0" };
        var routeValues = new { id = createdResource.Id, version = createdResource.Version };

        return serviceResponse.Success
            ? CreatedAtRoute(nameof(GetUserByIdAsync), routeValues, createdResource)
            : BadRequest(serviceResponse.ErrorMessage);
    }
}
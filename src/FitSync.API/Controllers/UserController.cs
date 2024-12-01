using FitSync.API.Responses;
using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Interfaces;
using FitSync.Domain.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FitSync.API.Controllers;


[ApiController]
[Route("api/users")]
[SwaggerTag("Endpoints for user related operations")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet("{id:int}",Name = nameof(GetUserByIdAsync))]
    [SwaggerOperation("Get resource by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "The resource is returned", typeof(UserViewModel))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The resource is not found")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserController),
            nameof(GetUserByIdAsync));
        var serviceResponse = await _userService.GetUserByIdAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound(serviceResponse.ErrorMessage);
    }


    [HttpGet("{id:int}/included", Name = nameof(GetUserByIdIncludeAllAsync))]
    [SwaggerOperation("Get resource by id including all related resources")]
    [SwaggerResponse(StatusCodes.Status200OK, "The resource is returned", typeof(UserViewModelIncluded))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The resource is not found")]
    public async Task<IActionResult> GetUserByIdIncludeAllAsync(int id)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserController), nameof(GetUserByIdIncludeAllAsync));
        var serviceResponse = await _userService.GetUserByIdIncludeAllAsync(id);

        return serviceResponse.Success
            ? Ok(serviceResponse.Data)
            : NotFound(serviceResponse.ErrorMessage);
    }


    [HttpPost]
    [SwaggerOperation("Create a new resource")]
    [SwaggerResponse(StatusCodes.Status201Created, "The resource is created", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error is returned")]
    public async Task<IActionResult> CreateUserAsync([FromBody] AddUserDto addUserDto)
    {
        _logger.LogInformation("{controller} called within {action}", nameof(UserController), nameof(CreateUserAsync));
        var serviceResponse = await _userService.CreateUserAsync(addUserDto);
        // TODO: Add a Resource class to handle the creation of the CreatedAtRoute response
        var resource = Resource.Create(serviceResponse.Data);

        return serviceResponse.Success
            ? CreatedAtRoute(nameof(GetUserByIdAsync), resource.GetRouteValues(), resource.GetCreatedResource())
            : BadRequest(serviceResponse.ErrorMessage);
    }
}
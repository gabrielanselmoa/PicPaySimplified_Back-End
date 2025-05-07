using Microsoft.AspNetCore.Mvc;
using PicPaySimplified.Application.Dtos.Auth;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace PicPaySimplified.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISecurityService _securityService;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ISecurityService securityService, IAuthService authService,
        ILogger<AuthController> logger)
    {
        _securityService = securityService;
        _authService = authService;
        _logger = logger;
    }
    
    [HttpPost("sign-up")]
    [SwaggerOperation(Summary = "User registration", Description = "Registers a new user account.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User registered successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data or registration failed")]
    [SwaggerRequestExample(typeof(SignUpRequest), typeof(SignUpRequest))]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        try
        {
            var response = await _authService.SignUp(request);
            if (response.Success)
                return Ok(response);
            return BadRequest(response.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during user sign-up.");
            return BadRequest(e.Message);
        }
    }

    [HttpPost("sign-in")]
    [SwaggerOperation(Summary = "User login", Description = "Authenticates a user and returns authentication tokens.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User authenticated successfully", typeof(ResultModel<LoggedResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid credentials or login failed")]
    [SwaggerRequestExample(typeof(SignInRequest), typeof(SignInRequest))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoggedResponse))]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        try
        {
            var response = await _authService.SignIn(request);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during user sign-in.");
            return BadRequest(e.Message);
        }
    }
}
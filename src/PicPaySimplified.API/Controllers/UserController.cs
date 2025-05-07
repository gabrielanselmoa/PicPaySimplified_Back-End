using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PicPaySimplified.Application.Dtos.Users;
using PicPaySimplified.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.API.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger; 

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger; 
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all registered users.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Users returned successfully", typeof(ResultModel<List<UserResponse>>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Users not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
    public async Task<IActionResult> GetAll()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        
        _logger.LogInformation("Received request to get all users.");

        var users = await _userService.GetAll();

        if (users.Data == null || users.Data.Count == 0)
        {
            _logger.LogWarning("No users found when retrieving all users.");
            return NotFound(users);
        }

        _logger.LogInformation("Successfully retrieved {UserCount} users.", users.Data.Count);
        return Ok(users);
    }

    [HttpGet]
    [Route("{id}")]
    [SwaggerOperation(Summary = "Get user by Id", Description = "Retrieves a single user by their unique identifier.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User returned successfully", typeof(ResultModel<UserResponse>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
    public async Task<IActionResult> GetById(string id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        
        _logger.LogInformation("Received request to get user by Id: {UserId}", id);

        var user = await _userService.GetById(id);

        if (user.Data == null)
        {
            _logger.LogWarning("User with Id {UserId} not found.", id);
            return NotFound(user);
        }

        _logger.LogInformation("Successfully retrieved user with Id: {UserId}", id);
        return Ok(user);
    }

    [HttpGet]
    [Route("search/email")]
    [SwaggerOperation(Summary = "Get user by email", Description = "Retrieves a single user by their email address.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User returned successfully", typeof(ResultModel<UserResponse>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        
        _logger.LogInformation("Received request to get user by email: {UserEmail}", email);

        var user = await _userService.GetByEmail(email);

        if (user.Data == null)
        {
            _logger.LogWarning("User with email {UserEmail} not found.", email);
            return NotFound(user);
        }

        _logger.LogInformation("Successfully retrieved user with email: {UserEmail}", email);
        return Ok(user);
    }

    [HttpGet]
    [Route("search/personal-identifier")]
    [SwaggerOperation(Summary = "Get user by CPF/CNPJ", Description = "Retrieves a single user by their CPF or CNPJ number.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User returned successfully", typeof(ResultModel<UserResponse>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
    public async Task<IActionResult> GetByCpfOrCnpj([FromQuery] string document)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        
        _logger.LogInformation("Received request to get user by CPF/CNPJ: {PersonalDocument}", document);

        var user = await _userService.GetByCpf(document);

        if (user.Data == null)
        {
            _logger.LogWarning("User with CPF/CNPJ {PersonalDocument} not found.", document);
            return NotFound(user);
        }

        _logger.LogInformation("Successfully retrieved user with CPF/CNPJ: {PersonalDocument}", document);
        return Ok(user);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new user", Description = "Creates a new user with the provided details.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User created successfully",
        typeof(ResultModel<UserResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid user data provided")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "User already exists")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]   
    public async Task<IActionResult> Create([FromBody] UserRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        
        _logger.LogInformation("Received request to create a new user. Email: {UserEmail}", request.Email);
        var user = await _userService.Create(request);
        if (user.Success == false)
        {
            _logger.LogWarning("User creation failed for email {UserEmail}. Errors: {Errors}", request.Email,
                user.Message);
            return Conflict(user);
        }

        _logger.LogInformation("Successfully created user with Id: {UserId}", user.Data?.Id);
        return Ok(user);
    }

    [HttpPut]
    [Route("{id}")]
    [SwaggerOperation(Summary = "Update a user by Id",
        Description = "Updates an existing user's details by their unique identifier.")]
    [SwaggerResponse(StatusCodes.Status200OK, "User updated successfully", typeof(ResultModel<UserResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid user data provided")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
    public async Task<IActionResult> Update(string id, [FromBody] UserRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        
        _logger.LogInformation("Received request to update user with Id: {UserId}", id);
        var user = await _userService.Update(id, request);
        if (user.Data == null)
        {
            _logger.LogWarning("User with Id {UserId} not found or update failed.", id);
            return NotFound(user);
        }

        _logger.LogInformation("Successfully updated user with Id: {UserId}", id);
        return Ok(user);
    }

    [HttpDelete]
    [Route("{id}")]
    [SwaggerOperation(Summary = "Delete a user by Id", Description = "Deletes a user by their unique identifier.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]   
    public async Task<IActionResult> Delete(string id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User is not authenticated");
        
        _logger.LogInformation("Received request to delete user with Id: {UserId}", id);

        var user = await _userService.Delete(id);
        if (user.Success == false)
        {
            _logger.LogError("User deletion failed for Id {UserId}. Errors: {Errors}", id, user.Message);
            return NotFound(user);
        }

        _logger.LogInformation("Successfully deleted user with Id: {UserId}", id);
        return NoContent();
    }
}
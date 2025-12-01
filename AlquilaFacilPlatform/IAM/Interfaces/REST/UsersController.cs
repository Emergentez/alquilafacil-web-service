using System.Net.Mime;
using AlquilaFacilPlatform.IAM.Domain.Model.Queries;
using AlquilaFacilPlatform.IAM.Domain.Services;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Resources;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.IAM.Interfaces.REST;

/// <summary>
/// Controller for user management operations
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for user management")]
public class UsersController(
    IUserQueryService userQueryService, IUserCommandService userCommandService
    ) : ControllerBase
{
    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>The user details</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("{userId:int}")]
    [SwaggerOperation(Summary = "Get user by ID", Description = "Retrieves a specific user by their ID. Requires Admin role.")]
    [SwaggerResponse(200, "User retrieved successfully")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Forbidden - requires Admin role")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var user = await userQueryService.Handle(getUserByIdQuery);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of all users</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users in the system. Requires Admin role.")]
    [SwaggerResponse(200, "List of users retrieved successfully")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Forbidden - requires Admin role")]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }

    /// <summary>
    /// Get username by user ID
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>The username</returns>
    [Authorize]
    [HttpGet("get-username/{userId:int}")]
    [SwaggerOperation(Summary = "Get username by ID", Description = "Retrieves only the username for a specific user")]
    [SwaggerResponse(200, "Username retrieved successfully")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> GetUsernameById(int userId)
    {
        var getUsernameByIdQuery = new GetUsernameByIdQuery(userId);
        var username = await userQueryService.Handle(getUsernameByIdQuery);
        return Ok(username);
    }

    /// <summary>
    /// Update user information
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="updateUsernameResource">Updated user data</param>
    /// <returns>The updated user</returns>
    [Authorize]
    [HttpPut("{userId:int}")]
    [SwaggerOperation(Summary = "Update user", Description = "Updates user information (username)")]
    [SwaggerResponse(200, "User updated successfully")]
    [SwaggerResponse(400, "Invalid user data")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUsernameResource updateUsernameResource)
    {
        var updateUserCommand =
            UpdateUsernameCommandFromResourceAssembler.ToUpdateUsernameCommand(userId, updateUsernameResource);
        var user = await userCommandService.Handle(updateUserCommand);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }
}
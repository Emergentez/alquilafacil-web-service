using System.Net.Mime;
using AlquilaFacilPlatform.IAM.Domain.Model.Queries;
using AlquilaFacilPlatform.IAM.Domain.Services;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Resources;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilaFacilPlatform.IAM.Interfaces.REST;

/**
 * <summary>
 *     The users controller
 * </summary>
 * <remarks>
 *     This class is used to handle user requests
 * </remarks>
 */

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(
    IUserQueryService userQueryService, IUserCommandService userCommandService
    ) : ControllerBase
{
    
    
    /**
     * <summary>
     *     Get user by id endpoint. It allows to get a user by id
     * </summary>
     * <param name="userId">The user id</param>
     * <returns>The user resource</returns>
     */
    [Authorize(Roles = "Admin")]
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var user = await userQueryService.Handle(getUserByIdQuery);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }
    
    
    /**
     * <summary>
     *     Get all users endpoint. It allows to get all users
     * </summary>
     * <returns>The user resources</returns>
     */
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }
    
    [Authorize]
    [HttpGet("get-username/{userId:int}")]
    public async Task<IActionResult> GetUsernameById(int userId)
    {
        var getUsernameByIdQuery = new GetUsernameByIdQuery(userId);
        var username = await userQueryService.Handle(getUsernameByIdQuery);
        return Ok(username);
    }
    
    [Authorize]
    [HttpPut("{userId:int}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUsernameResource updateUsernameResource)
    {
        var updateUserCommand =
            UpdateUsernameCommandFromResourceAssembler.ToUpdateUsernameCommand(userId, updateUsernameResource);
        var user = await userCommandService.Handle(updateUserCommand);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }
}
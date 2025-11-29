using System.Net.Mime;
using AlquilaFacilPlatform.IAM.Domain.Services;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Resources;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.IAM.Interfaces.REST;

/// <summary>
/// Controller for user authentication operations
/// </summary>
/// <remarks>
/// Provides endpoints for user sign-in and sign-up operations.
/// JWT tokens are issued upon successful authentication.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Authentication endpoints for user sign-in and registration")]
public class AuthenticationController(IUserCommandService userCommandService) : ControllerBase
{
    /// <summary>
    /// Authenticate a user and obtain a JWT token
    /// </summary>
    /// <param name="signInResource">User credentials for authentication</param>
    /// <returns>Authenticated user information with JWT token</returns>
    /// <response code="200">Returns the authenticated user with JWT token</response>
    /// <response code="400">If the credentials are invalid or missing</response>
    /// <response code="500">If there's an internal server error</response>
    [HttpPost("sign-in")]
    [SwaggerOperation(
        Summary = "User Sign In",
        Description = "Authenticates a user with email and password credentials. Returns a JWT token for subsequent API calls.",
        OperationId = "SignIn")]
    [SwaggerResponse(200, "Authentication successful", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(400, "Invalid credentials or request format")]
    [SwaggerResponse(500, "Internal server error - Invalid email or password")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
        var authenticatedUser = await userCommandService.Handle(signInCommand);
        var resource =
            AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                authenticatedUser.token);
        return Ok(resource);
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    /// <param name="signUpResource">New user registration data</param>
    /// <returns>Confirmation message on successful registration</returns>
    /// <response code="200">User created successfully</response>
    /// <response code="400">If the registration data is invalid or user already exists</response>
    /// <response code="500">If there's an internal server error</response>
    [HttpPost("sign-up")]
    [SwaggerOperation(
        Summary = "User Sign Up",
        Description = "Registers a new user account with username, email, and password. Email must be unique in the system.",
        OperationId = "SignUp")]
    [SwaggerResponse(200, "User registered successfully")]
    [SwaggerResponse(400, "Invalid registration data or email already exists")]
    [SwaggerResponse(500, "Internal server error")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
        await userCommandService.Handle(signUpCommand);
        return Ok(new { message = "User created successfully"});
    }
}

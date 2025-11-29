using System.Net.Mime;
using AlquilaFacilPlatform.Profiles.Domain.Model.Queries;
using AlquilaFacilPlatform.Profiles.Domain.Services;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Profiles.Interfaces.REST;

/// <summary>
/// Controller for managing user profiles
/// </summary>
/// <remarks>
/// Provides operations for viewing and updating user profile information,
/// including personal details, subscription status, and bank accounts.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("User profile management - View and update profile information")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService)
    : ControllerBase
{
    /// <summary>
    /// Get user profile by user ID
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>The user profile</returns>
    /// <response code="200">Returns the user profile</response>
    /// <response code="404">Profile not found</response>
    [HttpGet("user/{userId}")]
    [SwaggerOperation(
        Summary = "Get Profile by User ID",
        Description = "Retrieves the complete profile information for a specific user including personal details and contact information.",
        OperationId = "GetProfileByUserId")]
    [SwaggerResponse(200, "Profile found", typeof(ProfileResource))]
    [SwaggerResponse(404, "Profile not found for the specified user")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        var profile = await profileQueryService.Handle(new GetProfileByUserIdQuery(userId));
        if (profile == null) return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }

    /// <summary>
    /// Get subscription status for a user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>The user's subscription status</returns>
    /// <response code="200">Returns the subscription status</response>
    [HttpGet("subscription-status/{userId}")]
    [SwaggerOperation(
        Summary = "Get Subscription Status",
        Description = "Retrieves the current subscription status for a user. Returns status information like Active, Pending, Expired, or Cancelled.",
        OperationId = "GetSubscriptionStatus")]
    [SwaggerResponse(200, "Subscription status retrieved successfully")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptionStatusByUserId(int userId)
    {
        var query = new GetSubscriptionStatusByUserIdQuery(userId);
        var subscriptionStatus = await profileQueryService.Handle(query);
        return Ok(subscriptionStatus);
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="updateProfileResource">Updated profile data</param>
    /// <returns>The updated profile</returns>
    /// <response code="200">Profile updated successfully</response>
    /// <response code="400">Invalid profile data</response>
    [HttpPut("{userId}")]
    [SwaggerOperation(
        Summary = "Update Profile",
        Description = "Updates the profile information for a user. Allows modification of personal details like name, phone, document number, etc.",
        OperationId = "UpdateProfile")]
    [SwaggerResponse(200, "Profile updated successfully", typeof(ProfileResource))]
    [SwaggerResponse(400, "Invalid profile data")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileResource updateProfileResource)
    {
        var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(userId, updateProfileResource);
        var result = await profileCommandService.Handle(updateProfileCommand);
        return Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    /// <summary>
    /// Get user bank accounts
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>List of user's bank accounts</returns>
    /// <response code="200">Returns the bank accounts</response>
    [HttpGet("bank-accounts/{userId}")]
    [SwaggerOperation(
        Summary = "Get Bank Accounts",
        Description = "Retrieves all bank accounts associated with a user's profile. Used for payment processing and refunds.",
        OperationId = "GetBankAccounts")]
    [SwaggerResponse(200, "Bank accounts retrieved successfully")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfileBankAccountsByUserId(int userId)
    {
        var query = new GetProfileBankAccountsByUserIdQuery(userId);
        var bankAccounts = await profileQueryService.Handle(query);
        return Ok(bankAccounts);
    }
}

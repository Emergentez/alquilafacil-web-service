using System.Net.Mime;
using AlquilaFacilPlatform.Profiles.Domain.Model.Queries;
using AlquilaFacilPlatform.Profiles.Domain.Services;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AlquilaFacilPlatform.Profiles.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProfilesController(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService)
    : ControllerBase
{
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        var profile = await profileQueryService.Handle(new GetProfileByUserIdQuery(userId));
        if (profile == null) return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }
    
    [HttpGet("subscription-status/{userId}")]
    public async Task<IActionResult> GetSubscriptionStatusByUserId(int userId)
    {
        var query = new GetSubscriptionStatusByUserIdQuery(userId);
        var subscriptionStatus = await profileQueryService.Handle(query);
        return Ok(subscriptionStatus);
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileResource updateProfileResource)
    {
        var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(userId, updateProfileResource);
        var result = await profileCommandService.Handle(updateProfileCommand);
        return Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(result));
    }
    
    [HttpGet("bank-accounts/{userId}")]
    public async Task<IActionResult> GetProfileBankAccountsByUserId(int userId)
    {
        var query = new GetProfileBankAccountsByUserIdQuery(userId);
        var bankAccounts = await profileQueryService.Handle(query);
        return Ok(bankAccounts);
    }
}
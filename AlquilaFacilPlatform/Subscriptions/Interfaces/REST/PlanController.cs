using System.Net.Mime;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Subscriptions.Interfaces.REST;

/// <summary>
/// Controller for subscription plans
/// </summary>
/// <remarks>
/// Provides access to available subscription plans that users can purchase to become rental space owners.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Subscription plans - View available subscription options")]
public class PlanController(IPlanQueryService planQueryService) : ControllerBase
{
    /// <summary>
    /// Get all subscription plans
    /// </summary>
    /// <returns>List of all available subscription plans</returns>
    /// <response code="200">Returns all plans</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Plans",
        Description = "Retrieves all available subscription plans with their prices and features. Public endpoint - no authentication required.",
        OperationId = "GetAllPlans")]
    [SwaggerResponse(200, "Plans retrieved successfully", typeof(IEnumerable<PlanResource>))]
    [ProducesResponseType(typeof(IEnumerable<PlanResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPlans()
    {
        var getAllPlansQuery = new GetAllPlansQuery();
        var plans = await planQueryService.Handle(getAllPlansQuery);
        var resources = plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}

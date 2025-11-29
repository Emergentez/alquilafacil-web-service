using System.Net.Mime;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Subscriptions.Interfaces.REST;

/// <summary>
/// Controller for managing user subscriptions
/// </summary>
/// <remarks>
/// Handles subscription lifecycle including creation, retrieval, and status management.
/// Admin role required for certain operations.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Subscription management - Create, view and manage user subscriptions")]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryServices subscriptionQueryServices)
    : ControllerBase
{
    /// <summary>
    /// Create a new subscription
    /// </summary>
    /// <param name="createSubscriptionResource">Subscription creation data</param>
    /// <returns>The created subscription</returns>
    /// <response code="201">Subscription created successfully</response>
    /// <response code="400">Invalid subscription data</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Subscription",
        Description = "Creates a new subscription for a user. Links the user to a specific plan and sets initial status to pending.",
        OperationId = "CreateSubscription")]
    [SwaggerResponse(201, "Subscription created successfully", typeof(SubscriptionResource))]
    [SwaggerResponse(400, "Invalid subscription data or creation failed")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionResource createSubscriptionResource)
    {
        var createSubscriptionCommand =
            CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(createSubscriptionResource);
        var subscription = await subscriptionCommandService.Handle(createSubscriptionCommand);
        if (subscription is null) return BadRequest();
        var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);

        return CreatedAtAction(nameof(GetSubscriptionById), new { subscriptionId = resource.Id }, resource);
    }

    /// <summary>
    /// Get all subscriptions (Admin only)
    /// </summary>
    /// <returns>List of all subscriptions in the system</returns>
    /// <response code="200">Returns all subscriptions</response>
    /// <response code="401">Unauthorized - JWT token required</response>
    /// <response code="403">Forbidden - Admin role required</response>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Subscriptions",
        Description = "Retrieves all subscriptions in the system. Requires Admin role. Useful for administrative oversight and reporting.",
        OperationId = "GetAllSubscriptions")]
    [SwaggerResponse(200, "Subscriptions retrieved successfully", typeof(IEnumerable<SubscriptionResource>))]
    [SwaggerResponse(401, "Unauthorized - Valid JWT token required")]
    [SwaggerResponse(403, "Forbidden - Admin role required")]
    [ProducesResponseType(typeof(IEnumerable<SubscriptionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        var getAllSubscriptionsQuery = new GetAllSubscriptionsQuery();
        var subscriptions = await subscriptionQueryServices.Handle(getAllSubscriptionsQuery);
        var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Get a specific subscription by ID
    /// </summary>
    /// <param name="subscriptionId">The subscription ID</param>
    /// <returns>The subscription details</returns>
    /// <response code="200">Returns the subscription</response>
    /// <response code="404">Subscription not found</response>
    [HttpGet("{subscriptionId}")]
    [SwaggerOperation(
        Summary = "Get Subscription by ID",
        Description = "Retrieves detailed information about a specific subscription by its ID.",
        OperationId = "GetSubscriptionById")]
    [SwaggerResponse(200, "Subscription found", typeof(SubscriptionResource))]
    [SwaggerResponse(404, "Subscription not found")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionById([FromRoute] int subscriptionId)
    {
        var subscription = await subscriptionQueryServices.Handle(new GetSubscriptionByIdQuery(subscriptionId));
        if (subscription == null) return NotFound();
        var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
        return Ok(resource);
    }

    /// <summary>
    /// Activate a subscription (Admin only)
    /// </summary>
    /// <param name="subscriptionId">The subscription ID to activate</param>
    /// <returns>The updated subscription</returns>
    /// <response code="200">Subscription activated successfully</response>
    /// <response code="401">Unauthorized - JWT token required</response>
    /// <response code="403">Forbidden - Admin role required</response>
    /// <response code="404">Subscription not found</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("{subscriptionId}")]
    [SwaggerOperation(
        Summary = "Activate Subscription",
        Description = "Activates a pending subscription. Changes status from Pending to Active. Requires Admin role.",
        OperationId = "ActivateSubscription")]
    [SwaggerResponse(200, "Subscription activated successfully", typeof(SubscriptionResource))]
    [SwaggerResponse(401, "Unauthorized - Valid JWT token required")]
    [SwaggerResponse(403, "Forbidden - Admin role required")]
    [SwaggerResponse(404, "Subscription not found")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActiveSubscriptionStatus(int subscriptionId)
    {
        var activeSubscriptionStatusCommand = new ActiveSubscriptionStatusCommand(subscriptionId);
        var subscription = await subscriptionCommandService.Handle(activeSubscriptionStatusCommand);
        if (subscription == null) return NotFound();
        var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
        return Ok(resource);
    }
}

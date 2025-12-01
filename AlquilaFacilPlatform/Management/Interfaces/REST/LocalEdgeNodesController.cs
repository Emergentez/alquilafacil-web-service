using System.Net.Mime;
using AlquilaFacilPlatform.Management.Domain.Model.Queries;
using AlquilaFacilPlatform.Management.Domain.Services;
using AlquilaFacilPlatform.Management.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Management.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Management.Interfaces.REST;

/// <summary>
/// Controller for managing IoT edge nodes associated with locals
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for IoT edge node management")]
public class LocalEdgeNodesController(ILocalEdgeNodeQueryService localEdgeNodeQueryService,
                                    ILocalEdgeNodeCommandService localEdgeNodeCommandService) : ControllerBase
{
    /// <summary>
    /// Create a new edge node for a local
    /// </summary>
    /// <param name="resource">Edge node creation data</param>
    /// <returns>The created edge node</returns>
    [Authorize(Roles = "Admin,Technician")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create edge node", Description = "Creates a new IoT edge node and associates it with a local. Requires Admin or Technician role.")]
    [SwaggerResponse(201, "Edge node created successfully")]
    [SwaggerResponse(400, "Invalid edge node data")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Forbidden - requires Admin or Technician role")]
    public async Task<IActionResult> CreateLocalEdgeNode([FromBody] CreateLocalEdgeNodeResource resource)
    {
        var command = CreateLocalEdgeNodeCommandFromResourceAssembler.ToCommandFromResource(resource);
        var localEdgeNode = await localEdgeNodeCommandService.Handle(command);
        if (localEdgeNode is null) return BadRequest();
        var localEdgeNodeResource = LocalEdgeNodeResourceFromEntityAssembler.ToResourceFromEntity(localEdgeNode);
        return StatusCode(201, localEdgeNodeResource);
    }

    /// <summary>
    /// Update an existing edge node
    /// </summary>
    /// <param name="localId">The local ID</param>
    /// <param name="resource">Edge node update data</param>
    /// <returns>The updated edge node</returns>
    [Authorize(Roles = "Admin,Technician")]
    [HttpPut("{localId:int}")]
    [SwaggerOperation(Summary = "Update edge node", Description = "Updates an existing IoT edge node configuration. Requires Admin or Technician role.")]
    [SwaggerResponse(200, "Edge node updated successfully")]
    [SwaggerResponse(400, "Invalid edge node data")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(403, "Forbidden - requires Admin or Technician role")]
    public async Task<IActionResult> UpdateLocalEdgeNode(int localId, [FromBody] UpdateLocalEdgeNodeResource resource)
    {
        var command = UpdateLocalEdgeNodeCommandFromResourceAssembler.ToCommandFromResource(localId, resource);
        var localEdgeNode = await localEdgeNodeCommandService.Handle(command);
        if (localEdgeNode is null) return BadRequest();
        var localEdgeNodeResource = LocalEdgeNodeResourceFromEntityAssembler.ToResourceFromEntity(localEdgeNode);
        return Ok(localEdgeNodeResource);
    }

    /// <summary>
    /// Get edge node by local ID
    /// </summary>
    /// <param name="localId">The local ID</param>
    /// <returns>The edge node details</returns>
    [Authorize]
    [HttpGet("local-id/{localId}")]
    [SwaggerOperation(Summary = "Get edge node by local ID", Description = "Retrieves the IoT edge node configuration for a specific local")]
    [SwaggerResponse(200, "Edge node retrieved successfully")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Edge node not found")]
    public async Task<IActionResult> GetLocalEdgeNodeByLocalId(int localId)
    {
        var query = new GetLocalEdgeNodeByLocalIdQuery(localId);
        var localEdgeNode = await localEdgeNodeQueryService.Handle(query);
        if (localEdgeNode is null) return NotFound();
        var localEdgeNodeResource = LocalEdgeNodeResourceFromEntityAssembler.ToResourceFromEntity(localEdgeNode);
        return Ok(localEdgeNodeResource);
    }
}
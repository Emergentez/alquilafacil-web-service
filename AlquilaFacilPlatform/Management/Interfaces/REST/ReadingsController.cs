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
/// Controller for managing IoT sensor readings
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for IoT sensor readings management")]
public class ReadingsController(IReadingQueryService readingQueryService, IReadingCommandService readingCommandService) : ControllerBase
{
    /// <summary>
    /// Create a new sensor reading
    /// </summary>
    /// <param name="resource">Sensor reading data</param>
    /// <returns>The created reading</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create sensor reading", Description = "Creates a new sensor reading from an IoT device (smoke, sound, capacity, movement)")]
    [SwaggerResponse(201, "Reading created successfully")]
    [SwaggerResponse(400, "Invalid reading data")]
    public async Task<IActionResult> CreateReading([FromBody] CreateReadingResource resource)
    {
        var createReadingCommand = CreateReadingCommandFromResourceAssembler.ToCommandFromResource(resource);
        var reading = await readingCommandService.Handle(createReadingCommand);
        if (reading is null) return BadRequest();
        var readingResource = ReadingResourceFromEntityAssembler.ToResourceFromEntity(reading);
        return StatusCode(201, readingResource);
    }

    /// <summary>
    /// Get all readings for a local
    /// </summary>
    /// <param name="localId">The local ID</param>
    /// <returns>List of sensor readings</returns>
    [Authorize]
    [HttpGet("local-id/{localId}")]
    [SwaggerOperation(Summary = "Get readings by local ID", Description = "Retrieves all sensor readings for a specific local")]
    [SwaggerResponse(200, "List of readings retrieved successfully")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> GetReadingsBySensorId(int localId)
    {
        var getReadingsByLocalIdQuery = new GetAllReadingsByLocalIdQuery(localId);
        var readings = await readingQueryService.Handle(getReadingsByLocalIdQuery);
        var readingResources = readings.Select(ReadingResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(readingResources);
    }
}
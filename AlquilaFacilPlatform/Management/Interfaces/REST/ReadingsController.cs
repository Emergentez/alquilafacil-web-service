using System.Net.Mime;
using AlquilaFacilPlatform.Management.Domain.Model.Queries;
using AlquilaFacilPlatform.Management.Domain.Services;
using AlquilaFacilPlatform.Management.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Management.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilaFacilPlatform.Management.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReadingsController(IReadingQueryService readingQueryService, IReadingCommandService readingCommandService): ControllerBase
{
    
     [HttpPost]
     public async Task<IActionResult> CreateReading([FromBody] CreateReadingResource resource)
     {
         var createReadingCommand = CreateReadingCommandFromResourceAssembler.ToCommandFromResource(resource);
         var reading = await readingCommandService.Handle(createReadingCommand);
         if (reading is null) return BadRequest();
         var readingResource = ReadingResourceFromEntityAssembler.ToResourceFromEntity(reading);
         return StatusCode(201, readingResource);
     }
     
     [Authorize]
     [HttpGet("local-id/{localId}")]
     public async Task<IActionResult> GetReadingsBySensorId(int localId)
     {
         var getReadingsByLocalIdQuery = new GetAllReadingsByLocalIdQuery(localId);
         var readings = await readingQueryService.Handle(getReadingsByLocalIdQuery);
         var readingResources = readings.Select(ReadingResourceFromEntityAssembler.ToResourceFromEntity);
         return Ok(readingResources);
     }
}
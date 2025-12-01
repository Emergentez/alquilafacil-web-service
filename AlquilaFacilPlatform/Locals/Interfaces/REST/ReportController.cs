using System.Net.Mime;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST;

/// <summary>
/// Controller for managing incident reports on locals
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for incident report management")]
public class ReportController(IReportQueryService reportQueryService, IReportCommandService reportCommandService) : ControllerBase
{
    /// <summary>
    /// Create a new incident report
    /// </summary>
    /// <param name="createReportResource">Report creation data</param>
    /// <returns>The created report</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create incident report", Description = "Creates a new incident report for a local (noise complaints, damages, etc.)")]
    [SwaggerResponse(201, "Report created successfully")]
    [SwaggerResponse(400, "Invalid report data")]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportResource createReportResource)
    {
        var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(createReportResource);
        var report = await reportCommandService.Handle(command);
        if (report is null) return BadRequest();
        var reportResource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
        return StatusCode(201, reportResource);
    }

    /// <summary>
    /// Get reports by user ID
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>List of reports made by the user</returns>
    [HttpGet("get-by-user-id/{userId:int}")]
    [SwaggerOperation(Summary = "Get reports by user ID", Description = "Retrieves all incident reports created by a specific user")]
    [SwaggerResponse(200, "List of reports retrieved successfully")]
    public async Task<IActionResult> GetReportsByUserId(int userId)
    {
        var query = new GetReportsByUserIdQuery(userId);
        var reports = await reportQueryService.Handle(query);
        var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reportResources);
    }

    /// <summary>
    /// Get reports by local ID
    /// </summary>
    /// <param name="localId">The local ID</param>
    /// <returns>List of reports for the local</returns>
    [HttpGet("get-by-local-id/{localId:int}")]
    [SwaggerOperation(Summary = "Get reports by local ID", Description = "Retrieves all incident reports for a specific local")]
    [SwaggerResponse(200, "List of reports retrieved successfully")]
    public async Task<IActionResult> GetReportsByLocalId(int localId)
    {
        var query = new GetReportsByLocalIdQuery(localId);
        var reports = await reportQueryService.Handle(query);
        var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reportResources);
    }

    /// <summary>
    /// Delete a report
    /// </summary>
    /// <param name="reportId">The report ID</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{reportId:int}")]
    [SwaggerOperation(Summary = "Delete report", Description = "Deletes an incident report by its ID")]
    [SwaggerResponse(200, "Report deleted successfully")]
    [SwaggerResponse(404, "Report not found")]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        var command = new DeleteReportCommand(reportId);
        var reportDeleted = await reportCommandService.Handle(command);
        return StatusCode(200, reportDeleted);
    }
}
using System.Net.Mime;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST;

/// <summary>
/// Controller for local categories
/// </summary>
/// <remarks>
/// Provides access to predefined rental space categories like beach houses, country houses, urban houses, and elegant halls.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Local categories - View available rental space types")]
public class LocalCategoriesController(ILocalCategoryQueryService localCategoryQueryService)
    : ControllerBase
{
    /// <summary>
    /// Get all local categories
    /// </summary>
    /// <returns>List of all available local categories</returns>
    /// <response code="200">Returns all categories</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Local Categories",
        Description = "Retrieves all available rental space categories. Categories include: Casa de playa, Casa de campo, Casa urbana, Salon elegante. Public endpoint - no authentication required.",
        OperationId = "GetAllLocalCategories")]
    [SwaggerResponse(200, "Categories retrieved successfully", typeof(IEnumerable<LocalCategoryResource>))]
    [ProducesResponseType(typeof(IEnumerable<LocalCategoryResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLocalCategories()
    {
        var getAllLocalCategoriesQuery = new GetAllLocalCategoriesQuery();
        var localCategories = await localCategoryQueryService.Handle(getAllLocalCategoriesQuery);
        var localCategoryResources = localCategories.Select(LocalCategoryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localCategoryResources);
    }
}

using System.Net.Mime;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST;

/// <summary>
/// Controller for managing rental spaces (locals)
/// </summary>
/// <remarks>
/// Provides CRUD operations for rental spaces including search and filtering capabilities.
/// Most endpoints require authentication via JWT token.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Rental spaces management - Create, read, update and search for rental locals")]
public class LocalsController(ILocalCommandService localCommandService, ILocalQueryService localQueryService)
    : ControllerBase
{
    /// <summary>
    /// Create a new rental space
    /// </summary>
    /// <param name="resource">Local creation data</param>
    /// <returns>The created local resource</returns>
    /// <response code="201">Local created successfully</response>
    /// <response code="400">Invalid local data</response>
    /// <response code="401">Unauthorized - JWT token required</response>
    [Authorize]
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Local",
        Description = "Creates a new rental space. Requires authentication. Owner must have an active subscription.",
        OperationId = "CreateLocal")]
    [SwaggerResponse(201, "Local created successfully", typeof(LocalResource))]
    [SwaggerResponse(400, "Invalid local data or creation failed")]
    [SwaggerResponse(401, "Unauthorized - Valid JWT token required")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateLocal(CreateLocalResource resource)
    {
        var createLocalCommand = CreateLocalCommandFromResourceAssembler.ToCommandFromResource(resource);
        var local = await localCommandService.Handle(createLocalCommand);
        if (local is null) return BadRequest();
        var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
        return CreatedAtAction(nameof(GetLocalById), new { localId = localResource.Id }, localResource);
    }

    /// <summary>
    /// Get all available rental spaces
    /// </summary>
    /// <returns>List of all locals</returns>
    /// <response code="200">Returns the list of all locals</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Locals",
        Description = "Retrieves all rental spaces available in the platform. Public endpoint - no authentication required.",
        OperationId = "GetAllLocals")]
    [SwaggerResponse(200, "List of locals retrieved successfully", typeof(IEnumerable<LocalResource>))]
    [ProducesResponseType(typeof(IEnumerable<LocalResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLocals()
    {
        var getAllLocalsQuery = new GetAllLocalsQuery();
        var locals = await localQueryService.Handle(getAllLocalsQuery);
        var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localResources);
    }

    /// <summary>
    /// Get a specific rental space by ID
    /// </summary>
    /// <param name="localId">The unique identifier of the local</param>
    /// <returns>The local details</returns>
    /// <response code="200">Returns the local details</response>
    /// <response code="401">Unauthorized - JWT token required</response>
    /// <response code="404">Local not found</response>
    [Authorize]
    [HttpGet("{localId:int}")]
    [SwaggerOperation(
        Summary = "Get Local by ID",
        Description = "Retrieves detailed information about a specific rental space by its ID. Requires authentication.",
        OperationId = "GetLocalById")]
    [SwaggerResponse(200, "Local found", typeof(LocalResource))]
    [SwaggerResponse(401, "Unauthorized - Valid JWT token required")]
    [SwaggerResponse(404, "Local not found")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocalById(int localId)
    {
        var getLocalByIdQuery = new GetLocalByIdQuery(localId);
        var local = await localQueryService.Handle(getLocalByIdQuery);
        if (local == null) return NotFound();
        var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
        return Ok(localResource);
    }

    /// <summary>
    /// Update an existing rental space
    /// </summary>
    /// <param name="localId">The unique identifier of the local to update</param>
    /// <param name="resource">Updated local data</param>
    /// <returns>The updated local resource</returns>
    /// <response code="200">Local updated successfully</response>
    /// <response code="400">Invalid update data</response>
    /// <response code="401">Unauthorized - JWT token required</response>
    [Authorize]
    [HttpPut("{localId:int}")]
    [SwaggerOperation(
        Summary = "Update Local",
        Description = "Updates an existing rental space information. Only the owner can update their locals. Requires authentication.",
        OperationId = "UpdateLocal")]
    [SwaggerResponse(200, "Local updated successfully", typeof(LocalResource))]
    [SwaggerResponse(400, "Invalid update data or update failed")]
    [SwaggerResponse(401, "Unauthorized - Valid JWT token required")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateLocal(int localId, UpdateLocalResource resource)
    {
        var updateLocalCommand = UpdateLocalCommandFromResourceAssembler.ToCommandFromResource(localId, resource);
        var local = await localCommandService.Handle(updateLocalCommand);
        if (local is null) return BadRequest();
        var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
        return Ok(localResource);
    }

    /// <summary>
    /// Search locals by category and capacity range
    /// </summary>
    /// <param name="categoryId">The category ID to filter by</param>
    /// <param name="minCapacity">Minimum capacity required</param>
    /// <param name="maxCapacity">Maximum capacity allowed</param>
    /// <returns>Filtered list of locals</returns>
    /// <response code="200">Returns filtered locals</response>
    /// <response code="401">Unauthorized - JWT token required</response>
    [Authorize]
    [HttpGet("search-by-category-id-capacity-range/{categoryId:int}/{minCapacity:int}/{maxCapacity:int}")]
    [SwaggerOperation(
        Summary = "Search Locals by Category and Capacity",
        Description = "Searches for rental spaces filtered by category ID and capacity range. Useful for finding spaces that match specific event requirements.",
        OperationId = "SearchByCategoryAndCapacity")]
    [SwaggerResponse(200, "Filtered locals retrieved", typeof(IEnumerable<LocalResource>))]
    [SwaggerResponse(401, "Unauthorized - Valid JWT token required")]
    [ProducesResponseType(typeof(IEnumerable<LocalResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SearchByCategoryIdAndCapacityRange(int categoryId, int minCapacity, int maxCapacity)
    {
        var searchByCategoryIdAndCapacityRangeQuery = new GetLocalsByCategoryIdAndCapacityRangeQuery(categoryId, minCapacity, maxCapacity);
        var locals = await localQueryService.Handle(searchByCategoryIdAndCapacityRangeQuery);
        var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localResources);
    }

    /// <summary>
    /// Get all available districts
    /// </summary>
    /// <returns>List of all districts where locals are available</returns>
    /// <response code="200">Returns list of districts</response>
    /// <response code="401">Unauthorized - JWT token required</response>
    [Authorize]
    [HttpGet("get-all-districts")]
    [SwaggerOperation(
        Summary = "Get All Districts",
        Description = "Retrieves a list of all districts where rental spaces are available. Useful for location-based filtering.",
        OperationId = "GetAllDistricts")]
    [SwaggerResponse(200, "Districts retrieved successfully", typeof(IEnumerable<string>))]
    [SwaggerResponse(401, "Unauthorized - Valid JWT token required")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllDistricts()
    {
        var getAllLocalDistrictsQuery = new GetAllLocalDistrictsQuery();
        var districts = await localQueryService.Handle(getAllLocalDistrictsQuery);
        return Ok(districts);
    }

    /// <summary>
    /// Get all locals owned by a specific user
    /// </summary>
    /// <param name="userId">The user ID to get locals for</param>
    /// <returns>List of locals owned by the user</returns>
    /// <response code="200">Returns user's locals</response>
    /// <response code="401">Unauthorized - JWT token required</response>
    [Authorize]
    [HttpGet("get-user-locals/{userId:int}")]
    [SwaggerOperation(
        Summary = "Get User Locals",
        Description = "Retrieves all rental spaces owned by a specific user. Requires authentication.",
        OperationId = "GetUserLocals")]
    [SwaggerResponse(200, "User locals retrieved successfully", typeof(IEnumerable<LocalResource>))]
    [SwaggerResponse(401, "Unauthorized - Valid JWT token required")]
    [ProducesResponseType(typeof(IEnumerable<LocalResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserLocals(int userId)
    {
        var getUserLocalsQuery = new GetLocalsByUserIdQuery(userId);
        var locals = await localQueryService.Handle(getUserLocalsQuery);
        var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localResources);
    }
}

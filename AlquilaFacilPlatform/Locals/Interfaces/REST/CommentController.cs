using System.Net.Mime;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST;

/// <summary>
/// Controller for managing comments and reviews on rental spaces
/// </summary>
/// <remarks>
/// Provides functionality for users to leave comments and ratings on rental spaces they have visited.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Comments and reviews - View and create reviews for rental spaces")]
public class CommentController(ICommentCommandService commandService, ICommentQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Get all comments for a specific local
    /// </summary>
    /// <param name="localId">The local ID to get comments for</param>
    /// <returns>List of comments for the local</returns>
    /// <response code="200">Returns the comments</response>
    [HttpGet("local/{localId:int}")]
    [SwaggerOperation(
        Summary = "Get Comments by Local ID",
        Description = "Retrieves all comments and reviews for a specific rental space. Includes user ratings and feedback.",
        OperationId = "GetCommentsByLocalId")]
    [SwaggerResponse(200, "Comments retrieved successfully", typeof(IEnumerable<CommentResource>))]
    [ProducesResponseType(typeof(IEnumerable<CommentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCommentsByLocalId(int localId)
    {
        var getAllCommentsByLocalIdQuery = new GetAllCommentsByLocalIdQuery(localId);
        var comments = await queryService.Handle(getAllCommentsByLocalIdQuery);
        var commentsResources = comments.Select(CommentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(commentsResources);
    }

    /// <summary>
    /// Create a new comment
    /// </summary>
    /// <param name="resource">Comment creation data including rating and text</param>
    /// <returns>The created comment</returns>
    /// <response code="201">Comment created successfully</response>
    /// <response code="400">Invalid comment data</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Comment",
        Description = "Creates a new comment/review for a rental space. Users can provide a rating and text feedback.",
        OperationId = "CreateComment")]
    [SwaggerResponse(201, "Comment created successfully", typeof(CommentResource))]
    [SwaggerResponse(400, "Invalid comment data or creation failed")]
    [ProducesResponseType(typeof(CommentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateComment(CreateCommentResource resource)
    {
        var createCommentCommand = CreateCommentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var comment = await commandService.Handle(createCommentCommand);
        if (comment is null) return BadRequest();
        var commentResource = CommentResourceFromEntityAssembler.ToResourceFromEntity(comment);
        return StatusCode(201, commentResource);
    }
}

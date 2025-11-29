using System.Net.Mime;
using AlquilaFacilPlatform.Notifications.Domain.Models.Commands;
using AlquilaFacilPlatform.Notifications.Domain.Models.Queries;
using AlquilaFacilPlatform.Notifications.Domain.Services;
using AlquilaFacilPlatform.Notifications.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Notifications.Interfaces.REST.Transforms;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Notifications.Interfaces.REST;

/// <summary>
/// Controller for managing user notifications
/// </summary>
/// <remarks>
/// Handles notification retrieval and deletion for users.
/// Notifications are created automatically by the system for events like reservations and subscription updates.
/// </remarks>
[Produces(MediaTypeNames.Application.Json)]
[ApiController]
[Route("api/v1/[controller]")]
[SwaggerTag("Notification management - View and delete user notifications")]
public class NotificationController(INotificationCommandService notificationCommandService, INotificationQueryService notificationQueryService) : ControllerBase
{
    /// <summary>
    /// Get all notifications for a user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>List of user's notifications</returns>
    /// <response code="200">Returns the list of notifications</response>
    [HttpGet("{userId}")]
    [SwaggerOperation(
        Summary = "Get Notifications by User ID",
        Description = "Retrieves all notifications for a specific user. Returns notifications ordered by creation date. Includes reservation confirmations, subscription updates, and system alerts.",
        OperationId = "GetNotificationsByUserId")]
    [SwaggerResponse(200, "Notifications retrieved successfully", typeof(IEnumerable<NotificationResource>))]
    [ProducesResponseType(typeof(IEnumerable<NotificationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotificationsByUserId(int userId)
    {
        var query = new GetNotificationsByUserIdQuery(userId);
        var notifications = await notificationQueryService.Handle(query);
        var notificationResources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(notificationResources);
    }

    /// <summary>
    /// Delete a notification
    /// </summary>
    /// <param name="notificationId">The notification ID to delete</param>
    /// <returns>The deleted notification</returns>
    /// <response code="200">Notification deleted successfully</response>
    /// <response code="500">Server error - notification not found</response>
    [HttpDelete("{notificationId}")]
    [SwaggerOperation(
        Summary = "Delete Notification",
        Description = "Permanently deletes a notification from the system. This action cannot be undone.",
        OperationId = "DeleteNotification")]
    [SwaggerResponse(200, "Notification deleted successfully")]
    [SwaggerResponse(500, "Server error - Notification not found")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteNotification(int notificationId)
    {
        var command = new DeleteNotificationCommand(notificationId);
        var notification = await notificationCommandService.Handle(command);
        return StatusCode(200, notification);
    }
}

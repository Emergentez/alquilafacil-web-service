using System.Net.Mime;
using AlquilaFacilPlatform.Booking.Domain.Model.Queries;
using AlquilaFacilPlatform.Booking.Domain.Services;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Booking.Interfaces.REST;

/// <summary>
/// Controller for managing reservations
/// </summary>
/// <remarks>
/// Provides CRUD operations for booking reservations on rental spaces.
/// Handles reservation lifecycle from creation to deletion.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Reservation management - Create, update, delete and query reservations")]
public class ReservationController(IReservationCommandService reservationCommandService, IReservationQueryService reservationQueryService) : ControllerBase
{
    /// <summary>
    /// Create a new reservation
    /// </summary>
    /// <param name="resource">Reservation creation data including dates, local ID, and user ID</param>
    /// <returns>The created reservation</returns>
    /// <response code="201">Reservation created successfully</response>
    /// <response code="400">Invalid reservation data</response>
    /// <response code="500">Server error - validation failed (dates, user, or local issues)</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Reservation",
        Description = "Creates a new reservation for a rental space. Validates user existence, local existence, and date validity (start date must be before end date and in the future).",
        OperationId = "CreateReservation")]
    [SwaggerResponse(201, "Reservation created successfully", typeof(ReservationResource))]
    [SwaggerResponse(400, "Invalid reservation data")]
    [SwaggerResponse(500, "Server error - User not found, Local not found, or Invalid dates")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReservationAsync([FromBody] CreateReservationResource resource)
    {
        var command = CreateReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await reservationCommandService.Handle(command);
        var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(result);
        return StatusCode(201, reservationResource);
    }

    /// <summary>
    /// Update an existing reservation's dates
    /// </summary>
    /// <param name="id">The reservation ID to update</param>
    /// <param name="resource">Updated reservation dates</param>
    /// <returns>The updated reservation</returns>
    /// <response code="200">Reservation updated successfully</response>
    /// <response code="400">Invalid update data</response>
    /// <response code="500">Server error - validation failed</response>
    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Update Reservation Dates",
        Description = "Updates the start and end dates of an existing reservation. Validates that new dates are valid and in the future.",
        OperationId = "UpdateReservation")]
    [SwaggerResponse(200, "Reservation updated successfully", typeof(ReservationResource))]
    [SwaggerResponse(400, "Invalid update data")]
    [SwaggerResponse(500, "Server error - Invalid dates or reservation not found")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateReservationAsync(int id, [FromBody] UpdateReservationResource resource)
    {
        var command = UpdateReservationDateCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await reservationCommandService.Handle(command);
        var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(reservationResource);
    }

    /// <summary>
    /// Delete a reservation
    /// </summary>
    /// <param name="id">The reservation ID to delete</param>
    /// <returns>Confirmation message</returns>
    /// <response code="200">Reservation deleted successfully</response>
    /// <response code="500">Server error - reservation not found</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete Reservation",
        Description = "Permanently deletes a reservation from the system. This action cannot be undone.",
        OperationId = "DeleteReservation")]
    [SwaggerResponse(200, "Reservation deleted successfully")]
    [SwaggerResponse(500, "Server error - Reservation does not exist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReservationAsync(int id)
    {
        var resource = new DeleteReservationResource(id);
        var command = DeleteReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
        await reservationCommandService.Handle(command);
        return StatusCode(200, "Reservation deleted");
    }

    /// <summary>
    /// Get all reservations for a specific user
    /// </summary>
    /// <param name="userId">The user ID to get reservations for</param>
    /// <returns>List of user's reservations</returns>
    /// <response code="200">Returns the list of reservations</response>
    [HttpGet("by-user-id/{userId:int}")]
    [SwaggerOperation(
        Summary = "Get Reservations by User ID",
        Description = "Retrieves all reservations made by a specific user. Returns an empty list if no reservations found.",
        OperationId = "GetReservationsByUserId")]
    [SwaggerResponse(200, "Reservations retrieved successfully", typeof(IEnumerable<ReservationResource>))]
    [ProducesResponseType(typeof(IEnumerable<ReservationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReservationsByUserIdAsync(int userId)
    {
        var query = new GetReservationsByUserId(userId);
        var result = await reservationQueryService.GetReservationsByUserIdAsync(query);
        var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reservationResource);
    }

    /// <summary>
    /// Get reservation details for a local owner
    /// </summary>
    /// <param name="userId">The owner's user ID</param>
    /// <returns>Detailed reservation information including user and local details</returns>
    /// <response code="200">Returns reservation details</response>
    [HttpGet("reservation-user-details/{userId:int}")]
    [SwaggerOperation(
        Summary = "Get Reservation User Details",
        Description = "Retrieves detailed reservation information for a local owner, including guest information and booking details.",
        OperationId = "GetReservationUserDetails")]
    [SwaggerResponse(200, "Reservation details retrieved successfully", typeof(ReservationDetailsResource))]
    [ProducesResponseType(typeof(ReservationDetailsResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReservationUserDetailsAsync(int userId)
    {
        var query = new GetReservationsByOwnerIdQuery(userId);
        var reservations = await reservationQueryService.GetReservationsByOwnerIdAsync(query);
        var reservationDetailsResource = new ReservationDetailsResource(reservations);
        return Ok(reservationDetailsResource);
    }

    /// <summary>
    /// Get reservations by start date
    /// </summary>
    /// <param name="startDate">The start date to filter by (format: yyyy-MM-dd)</param>
    /// <returns>List of reservations starting on the specified date</returns>
    /// <response code="200">Returns matching reservations</response>
    [HttpGet("by-start-date/{startDate}")]
    [SwaggerOperation(
        Summary = "Get Reservations by Start Date",
        Description = "Retrieves all reservations that start on a specific date. Useful for scheduling and availability checks.",
        OperationId = "GetReservationsByStartDate")]
    [SwaggerResponse(200, "Reservations retrieved successfully", typeof(IEnumerable<ReservationResource>))]
    [ProducesResponseType(typeof(IEnumerable<ReservationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReservationByStartDateAsync(DateTime startDate)
    {
        var query = new GetReservationByStartDate(startDate);
        var result = await reservationQueryService.GetReservationByStartDateAsync(query);
        var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reservationResource);
    }

    /// <summary>
    /// Get reservations by end date
    /// </summary>
    /// <param name="endDate">The end date to filter by (format: yyyy-MM-dd)</param>
    /// <returns>List of reservations ending on the specified date</returns>
    /// <response code="200">Returns matching reservations</response>
    [HttpGet("by-end-date/{endDate}")]
    [SwaggerOperation(
        Summary = "Get Reservations by End Date",
        Description = "Retrieves all reservations that end on a specific date. Useful for checkout management and turnover planning.",
        OperationId = "GetReservationsByEndDate")]
    [SwaggerResponse(200, "Reservations retrieved successfully", typeof(IEnumerable<ReservationResource>))]
    [ProducesResponseType(typeof(IEnumerable<ReservationResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReservationByEndDateAsync(DateTime endDate)
    {
        var query = new GetReservationByEndDate(endDate);
        var result = await reservationQueryService.GetReservationByEndDateAsync(query);
        var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reservationResource);
    }
}

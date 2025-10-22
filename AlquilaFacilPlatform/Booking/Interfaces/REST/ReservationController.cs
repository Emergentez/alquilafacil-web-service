using System.Net.Mime;
using AlquilaFacilPlatform.Booking.Domain.Model.Queries;
using AlquilaFacilPlatform.Booking.Domain.Services;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AlquilaFacilPlatform.Booking.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReservationController(IReservationCommandService reservationCommandService, IReservationQueryService reservationQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReservationAsync([FromBody]CreateReservationResource resource)
    {
        var command = CreateReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await reservationCommandService.Handle(command);
        var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(result);
        return StatusCode(201, reservationResource);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReservationAsync(int id, [FromBody]UpdateReservationResource resource)
    {
        var command = UpdateReservationDateCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await reservationCommandService.Handle(command);
        var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(reservationResource);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservationAsync(int id)
    {
        var resource = new DeleteReservationResource(id);
        var command = DeleteReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
        await reservationCommandService.Handle(command);
        return StatusCode(200, "Reservation deleted");
    }
    

    [HttpGet("by-user-id/{userId:int}")]
    public async Task<IActionResult> GetReservationsByUserIdAsync(int userId)
    {
        var query = new GetReservationsByUserId(userId);
        var result = await reservationQueryService.GetReservationsByUserIdAsync(query);
        var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reservationResource);
    }

    [HttpGet("reservation-user-details/{userId:int}")]
    public async Task<IActionResult> GetReservationUserDetailsAsync(int userId)
    {
        var query = new GetReservationsByOwnerIdQuery(userId);
        var reservations = await reservationQueryService.GetReservationsByOwnerIdAsync(query);
        var reservationDetailsResource = new ReservationDetailsResource(reservations);
        return Ok(reservationDetailsResource);
    }

    [HttpGet("by-start-date/{startDate}")]
    public async Task<IActionResult> GetReservationByStartDateAsync(DateTime startDate)
    {
        var query = new GetReservationByStartDate(startDate);
        var result = await reservationQueryService.GetReservationByStartDateAsync(query);
        var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reservationResource);
    }

    [HttpGet("by-end-date/{endDate}")]
    public async Task<IActionResult> GetReservationByEndDateAsync(DateTime endDate)
    {
        var query = new GetReservationByEndDate(endDate);
        var result = await reservationQueryService.GetReservationByEndDateAsync(query);
        var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reservationResource);
    }
}

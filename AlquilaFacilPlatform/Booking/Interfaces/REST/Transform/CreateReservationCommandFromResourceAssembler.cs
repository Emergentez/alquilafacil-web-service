using AlquilaFacilPlatform.Booking.Domain.Model.Commands;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Booking.Interfaces.REST.Transform;

public static class CreateReservationCommandFromResourceAssembler
{
    public static CreateReservationCommand ToCommandFromResource( CreateReservationResource resource)
    {
        return new CreateReservationCommand(
            resource.StartDate,
            resource.EndDate,
            resource.UserId,
            resource.LocalId,
            resource.price,
            resource.voucherImageUrl
        );
    }
}
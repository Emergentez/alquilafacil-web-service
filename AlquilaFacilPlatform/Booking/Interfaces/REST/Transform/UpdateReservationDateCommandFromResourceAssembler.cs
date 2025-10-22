using AlquilaFacilPlatform.Booking.Domain.Model.Commands;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Booking.Interfaces.REST.Transform;

public static class UpdateReservationDateCommandFromResourceAssembler
{
    public static UpdateReservationDateCommand ToCommandFromResource(int id,UpdateReservationResource resource)
    {
        return new UpdateReservationDateCommand(
            id,
            resource.StartDate,
            resource.EndDate
        );
    }
}


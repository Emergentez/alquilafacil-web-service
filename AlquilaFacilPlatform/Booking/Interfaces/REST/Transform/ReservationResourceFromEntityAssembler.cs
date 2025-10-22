using AlquilaFacilPlatform.Booking.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Booking.Interfaces.REST.Transform;

public static class ReservationResourceFromEntityAssembler
{
    public static ReservationResource ToResourceFromEntity(Reservation entity)
    {
        return new ReservationResource
        (
            entity.Id,
            entity.StartDate,
            entity.EndDate,
            entity.UserId,
            entity.LocalId
        );
    }
}
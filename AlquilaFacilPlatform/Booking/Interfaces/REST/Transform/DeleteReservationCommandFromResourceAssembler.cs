using AlquilaFacilPlatform.Booking.Domain.Model.Commands;
using AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Booking.Interfaces.REST.Transform;

public static class DeleteReservationCommandFromResourceAssembler
{
    public static DeleteReservationCommand ToCommandFromResource(DeleteReservationResource resource)
    {
        return new DeleteReservationCommand(resource.Id);
    }
}
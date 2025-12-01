using AlquilaFacilPlatform.Booking.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Booking.Domain.Services;

public interface ISeedDemoReservationCommandService
{
    Task Handle(SeedDemoReservationCommand command);
}

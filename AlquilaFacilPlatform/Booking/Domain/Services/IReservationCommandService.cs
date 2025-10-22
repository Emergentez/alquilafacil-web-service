using AlquilaFacilPlatform.Booking.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Booking.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Booking.Domain.Services;

public interface IReservationCommandService
{
    Task<Reservation> Handle(CreateReservationCommand reservation);
    Task<Reservation> Handle(UpdateReservationDateCommand reservation);
    Task<Reservation> Handle(DeleteReservationCommand reservation);
}
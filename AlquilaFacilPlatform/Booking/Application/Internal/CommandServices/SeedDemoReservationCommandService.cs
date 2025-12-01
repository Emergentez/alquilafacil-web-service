using AlquilaFacilPlatform.Booking.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Booking.Domain.Model.Commands;
using AlquilaFacilPlatform.Booking.Domain.Repositories;
using AlquilaFacilPlatform.Booking.Domain.Services;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Booking.Application.Internal.CommandServices;

public class SeedDemoReservationCommandService(
    IReservationRepository reservationRepository,
    IUnitOfWork unitOfWork) : ISeedDemoReservationCommandService
{
    public async Task Handle(SeedDemoReservationCommand command)
    {
        var existingReservations = await reservationRepository.GetReservationsByUserIdAsync(2);
        if (existingReservations.Any()) return;

        // Reserva para el miércoles 3 de diciembre 2025, todo el día
        // Local ID: 1 (Casa de Playa Miraflores)
        // Usuario arrendatario ID: 2
        var startDate = new DateTime(2025, 12, 3, 0, 0, 0);
        var endDate = new DateTime(2025, 12, 3, 23, 59, 59);

        var createReservationCommand = new CreateReservationCommand(
            StartDate: startDate,
            EndDate: endDate,
            UserId: 2,
            LocalId: 1,
            Price: 150 * 24,
            VoucherImageUrl: "https://images.unsplash.com/photo-1554224155-6726b3ff858f?w=800"
        );

        var reservation = new Reservation(createReservationCommand);
        await reservationRepository.AddAsync(reservation);
        await unitOfWork.CompleteAsync();
    }
}

namespace AlquilaFacilPlatform.Booking.Domain.Model.Commands;

public record CreateReservationCommand(
    DateTime StartDate,
    DateTime EndDate,
    int UserId,
    int LocalId,
    float Price,
    string VoucherImageUrl
    );
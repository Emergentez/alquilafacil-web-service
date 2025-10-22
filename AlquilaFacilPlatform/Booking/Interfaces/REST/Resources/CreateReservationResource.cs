namespace AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;

public record CreateReservationResource(DateTime StartDate, DateTime EndDate, int UserId, int LocalId, float price, string voucherImageUrl);
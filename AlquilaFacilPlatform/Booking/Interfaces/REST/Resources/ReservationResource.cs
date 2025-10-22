namespace AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;

public record ReservationResource(int Id,DateTime StartDate, DateTime EndDate, int UserId, int LocalId);
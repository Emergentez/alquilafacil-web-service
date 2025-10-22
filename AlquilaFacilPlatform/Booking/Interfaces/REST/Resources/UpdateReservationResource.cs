namespace AlquilaFacilPlatform.Booking.Interfaces.REST.Resources;

public record UpdateReservationResource(DateTime StartDate, DateTime EndDate, int UserId, int LocalId);
namespace AlquilaFacilPlatform.Booking.Domain.Model.Commands;

public record UpdateReservationDateCommand(int Id, DateTime StartDate, DateTime EndDate);
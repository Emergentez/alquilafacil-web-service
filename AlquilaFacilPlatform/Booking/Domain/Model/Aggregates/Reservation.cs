using AlquilaFacilPlatform.Booking.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Booking.Domain.Model.Aggregates;

public partial class Reservation
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; }
    public int LocalId { get; }
    public float Price { get; set;}
    public string VoucherImageUrl { get; set; }
}

public partial class Reservation
{
    public Reservation()
    {
        UserId = 0;
        LocalId = 0;
        VoucherImageUrl = "";
    }
    
    public Reservation(CreateReservationCommand command)
    {
        StartDate = command.StartDate;
        EndDate = command.EndDate;
        UserId = command.UserId;
        LocalId = command.LocalId;
        Price = command.Price;
        VoucherImageUrl = command.VoucherImageUrl;
    }
    
    public void UpdateDate(UpdateReservationDateCommand command)
    {
        StartDate = command.StartDate;
        EndDate = command.EndDate;
    }
}
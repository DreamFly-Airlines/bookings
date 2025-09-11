namespace Bookings.Domain.Bookings.Entities;

public class BoardingPass
{
    public string TicketNo { get; set; } = null!;
    public int FlightId { get; set; }
    public int BoardingNo { get; set; }
    public string SeatNo { get; set; } = null!;
    public virtual TicketFlight TicketFlight { get; set; } = null!;
}

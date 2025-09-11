namespace Bookings.Domain.Bookings.Entities;

public class TicketFlight
{
    public string TicketNo { get; set; } = null!;
    public int FlightId { get; set; }
    public string FareConditions { get; set; } = null!;
    public decimal Amount { get; set; }

    public virtual BoardingPass? BoardingPass { get; set; }

    public virtual Flight Flight { get; set; } = null!;

    public virtual Ticket TicketNoNavigation { get; set; } = null!;
}

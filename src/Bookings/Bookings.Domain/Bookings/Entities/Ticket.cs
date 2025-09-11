namespace Bookings.Domain.Bookings.Entities;

public class Ticket
{
    public string TicketNo { get; set; } = null!;
    public string BookRef { get; set; } = null!;
    public string PassengerId { get; set; } = null!;
    public string PassengerName { get; set; } = null!;
    public string? ContactData { get; set; }
    public virtual Booking BookRefNavigation { get; set; } = null!;
    public virtual ICollection<TicketFlight> TicketFlights { get; set; } = new List<TicketFlight>();
}

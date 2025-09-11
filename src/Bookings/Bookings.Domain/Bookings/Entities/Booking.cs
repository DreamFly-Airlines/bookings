namespace Bookings.Domain.Bookings.Entities;

public class Booking
{
    public string BookRef { get; set; } = null!;
    public DateTime BookDate { get; set; }
    public decimal TotalAmount { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

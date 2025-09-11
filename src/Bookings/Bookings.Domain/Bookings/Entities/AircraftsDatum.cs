namespace Bookings.Domain.Bookings.Entities;

public class AircraftsDatum
{
    public string AircraftCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Range { get; set; }
    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}

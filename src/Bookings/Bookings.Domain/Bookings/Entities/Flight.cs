namespace Bookings.Domain.Bookings.Entities;

public class Flight
{
    public int FlightId { get; set; }
    public string FlightNo { get; set; } = null!;
    public DateTime ScheduledDeparture { get; set; }
    public DateTime ScheduledArrival { get; set; }
    public string DepartureAirport { get; set; } = null!;
    public string ArrivalAirport { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string AircraftCode { get; set; } = null!;
    public DateTime? ActualDeparture { get; set; }
    public DateTime? ActualArrival { get; set; }
    public virtual AircraftData AircraftCodeNavigation { get; set; } = null!;
    public virtual AirportsDatum ArrivalAirportNavigation { get; set; } = null!;
    public virtual AirportsDatum DepartureAirportNavigation { get; set; } = null!;
    public virtual ICollection<TicketFlight> TicketFlights { get; set; } = new List<TicketFlight>();
}

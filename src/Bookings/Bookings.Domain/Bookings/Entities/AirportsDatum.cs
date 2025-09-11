using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class AirportsDatum
{
    public string AirportCode { get; set; } = null!;
    public string AirportName { get; set; } = null!;
    public string City { get; set; } = null!;
    public Coordinates Coordinates { get; set; }
    public string Timezone { get; set; } = null!;
    public virtual ICollection<Flight> FlightArrivalAirportNavigations { get; set; } = new List<Flight>();
    public virtual ICollection<Flight> FlightDepartureAirportNavigations { get; set; } = new List<Flight>();
}

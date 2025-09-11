using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class Airport
{
    public string? AirportCode { get; set; }
    public string? AirportName { get; set; }
    public string? City { get; set; }
    public Coordinates? Coordinates { get; set; }
    public string? Timezone { get; set; }
}

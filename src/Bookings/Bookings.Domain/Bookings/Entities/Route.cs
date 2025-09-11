namespace Bookings.Domain.Bookings.Entities;

public class Route
{
    public string? FlightNo { get; set; }
    public string? DepartureAirport { get; set; }
    public string? DepartureAirportName { get; set; }
    public string? DepartureCity { get; set; }
    public string? ArrivalAirport { get; set; }
    public string? ArrivalAirportName { get; set; }
    public string? ArrivalCity { get; set; }
    public string? AircraftCode { get; set; }
    public TimeSpan? Duration { get; set; }
    public List<int>? DaysOfWeek { get; set; }
}

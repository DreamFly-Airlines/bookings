namespace Bookings.Domain.Bookings.Entities;

// TODO: it's used only for additional info for Flight
// maybe we should move it to infrastructure or application and make ReadModel

public class FlightsV
{
    public int? FlightId { get; set; }
    public string? FlightNo { get; set; }
    public DateTime? ScheduledDeparture { get; set; }
    public DateTime? ScheduledDepartureLocal { get; set; }
    public DateTime? ScheduledArrival { get; set; }
    public DateTime? ScheduledArrivalLocal { get; set; }
    public TimeSpan? ScheduledDuration { get; set; }
    public string? DepartureAirport { get; set; }
    public string? DepartureAirportName { get; set; }
    public string? DepartureCity { get; set; }
    public string? ArrivalAirport { get; set; }
    public string? ArrivalAirportName { get; set; }
    public string? ArrivalCity { get; set; }
    public string? Status { get; set; }
    public string? AircraftCode { get; set; }
    public DateTime? ActualDeparture { get; set; }
    public DateTime? ActualDepartureLocal { get; set; }
    public DateTime? ActualArrival { get; set; }
    public DateTime? ActualArrivalLocal { get; set; }
    public TimeSpan? ActualDuration { get; set; }
}

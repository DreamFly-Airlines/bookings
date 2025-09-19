using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.ReadModels.ReadModels;

public class FlightReadModel
{
    public int FlightId { get; init; }
    public FlightNo FlightNo { get; init; }
    public DateTime ScheduledDeparture { get; init; }
    public DateTime ScheduledDepartureLocal { get; init; }
    public DateTime ScheduledArrival { get; init; }
    public DateTime ScheduledArrivalLocal { get; init; }
    public TimeSpan ScheduledDuration { get; init; }
    public IataAirportCode DepartureAirport { get; init; }
    public string DepartureAirportName { get; init; } = null!;
    public string DepartureCity { get; init; } = null!;
    public IataAirportCode ArrivalAirport { get; init; }
    public string ArrivalAirportName { get; init; } = null!;
    public string ArrivalCity { get; init; } = null!;
    public FlightStatus Status { get; init; }
    public AircraftCode AircraftCode { get; init; }
    public DateTime? ActualDeparture { get; init; }
    public DateTime? ActualDepartureLocal { get; init; }
    public DateTime? ActualArrival { get; init; }
    public DateTime? ActualArrivalLocal { get; init; }
    public TimeSpan? ActualDuration { get; init; }
}

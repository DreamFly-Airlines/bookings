using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.ReadModels.ReadModels;

public class FlightReadModel
{
    public required int FlightId { get; init; }
    public required FlightNo FlightNo { get; init; }
    public required DateTime ScheduledDeparture { get; init; }
    public required DateTime ScheduledDepartureLocal { get; init; }
    public required DateTime ScheduledArrival { get; init; }
    public required DateTime ScheduledArrivalLocal { get; init; }
    public required TimeSpan ScheduledDuration { get; init; }
    public required IataAirportCode DepartureAirport { get; init; }
    public required string DepartureAirportName { get; init; }
    public required string DepartureCity { get; init; }
    public required IataAirportCode ArrivalAirport { get; init; }
    public required string ArrivalAirportName { get; init; }
    public required string ArrivalCity { get; init; } = null!;
    public required FlightStatus Status { get; init; }
    public required AircraftCode AircraftCode { get; init; }
    public DateTime? ActualDeparture { get; init; }
    public DateTime? ActualDepartureLocal { get; init; }
    public DateTime? ActualArrival { get; init; }
    public DateTime? ActualArrivalLocal { get; init; }
    public TimeSpan? ActualDuration { get; init; }
}

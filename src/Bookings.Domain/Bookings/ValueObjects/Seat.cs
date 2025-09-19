using Bookings.Domain.Bookings.Enums;

namespace Bookings.Domain.Bookings.ValueObjects;

public record Seat(AircraftCode AircraftCode, string SeatNo, FareConditions FareConditions);

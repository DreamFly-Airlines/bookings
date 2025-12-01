using Bookings.Domain.Bookings.Enums;

namespace Bookings.Application.Bookings.ReadModels;

public class TicketFlightReadModel
{
    public required FareConditions FareConditions { get; init; }
    public required decimal Amount { get; init; }
    public required FlightReadModel Flight { get; init; }
}
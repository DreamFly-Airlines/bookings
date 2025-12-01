using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.ReadModels;

public class TicketReadModel
{
    public required TicketNo TicketNo { get; init; }
    public required string PassengerName { get; init; }
    public required ContactData ContactData { get; init; }
    public required List<TicketFlightReadModel> TicketFlights { get; init; }
}
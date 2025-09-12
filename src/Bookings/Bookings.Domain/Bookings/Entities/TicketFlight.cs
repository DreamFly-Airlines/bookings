using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class TicketFlight
{
    public TicketNo TicketNo { get; }
    public int FlightId { get; }
    public FareConditions FareConditions { get; }
    public decimal Amount { get; }

    public TicketFlight(TicketNo ticketNo, int flightId, FareConditions fareConditions, decimal amount)
    {
        TicketNo = ticketNo;
        FlightId = flightId;
        FareConditions = fareConditions;
        Amount = amount;
    }
}

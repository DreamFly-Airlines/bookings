using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Domain.Shared.Exceptions;

namespace Bookings.Domain.Bookings.Entities;

public class Ticket
{
    public TicketNo TicketNo { get; }
    public BookRef BookRef { get; }
    public string PassengerId { get; }
    public string PassengerName { get; }
    public ContactData ContactData { get; }
    public IReadOnlySet<TicketFlight> TicketFlights { get; }

    private HashSet<TicketFlight> _ticketFlights;

    public Ticket(
        TicketNo ticketNo, 
        BookRef bookRef, 
        string passengerId, 
        string passengerName, 
        ContactData contactData)
    {
        TicketNo = ticketNo;
        BookRef = bookRef;
        PassengerId = passengerId;
        PassengerName = passengerName;
        ContactData = contactData;
    }

    public void AddFlight(int flightId, FareConditions fareConditions, decimal amount)
    {
        var ticketFlight = new TicketFlight(TicketNo, flightId, fareConditions, amount);
        if (!_ticketFlights.Add(ticketFlight))
            throw new InvalidDomainOperationException(
                $"{nameof(Flight)} with ID \"{flightId}\" already added to {nameof(Ticket)} " +
                $"with {nameof(TicketNo)} \"{TicketNo}\".");
    }
}

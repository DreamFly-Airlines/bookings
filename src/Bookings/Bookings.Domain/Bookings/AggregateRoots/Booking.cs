using Bookings.Domain.Bookings.Entities;
using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.Events;
using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Domain.Shared.Abstractions;
using Bookings.Domain.Shared.Exceptions;

namespace Bookings.Domain.Bookings.AggregateRoots;

public class Booking : AggregateRoot<IDomainEvent>
{
    public BookRef BookRef { get; }
    public DateTime BookDate { get; }
    public decimal TotalAmount { get; private set; }
    public IReadOnlySet<Ticket> Tickets => _tickets;

    private readonly HashSet<Ticket> _tickets;
    
    public Booking(
        BookRef bookRef, 
        DateTime bookDate,
        FareConditions fareConditions,
        IEnumerable<int> flightsIdsForTicket,
        IEnumerable<(
            TicketNo TicketNo, 
            decimal TicketCost, 
            string PassengerId, 
            string PassengerName, 
            ContactData ContactData)> ticketsInfo)
    {
        BookRef = bookRef;
        BookDate = bookDate;
        TotalAmount = 0;
        _tickets = [];
        foreach (var ticketInfo in ticketsInfo)
        {
            var ticket = new Ticket(
                ticketInfo.TicketNo, 
                BookRef, 
                ticketInfo.PassengerId, 
                ticketInfo.PassengerName, 
                ticketInfo.ContactData);
            AddTicketAndEvaluateTotalAmountOrThrow(ticket, ticketInfo.TicketCost);
            foreach (var flightId in flightsIdsForTicket)
                ticket.AddFlight(flightId, fareConditions, ticketInfo.TicketCost);
        }
        AddDomainEvent(new BookingMade(BookRef));
    }

    private void AddTicketAndEvaluateTotalAmountOrThrow(Ticket ticketNo, decimal ticketCost)
    {
        if (_tickets.Add(ticketNo))
            TotalAmount += ticketCost;
        else
            throw new InvalidDomainOperationException(
                $"A {nameof(Booking)} with {nameof(BookRef)} \"{BookRef}\" " +
                $"already has a {nameof(Ticket)} with {nameof(ticketNo)} \"{ticketNo}\".");
    }
}

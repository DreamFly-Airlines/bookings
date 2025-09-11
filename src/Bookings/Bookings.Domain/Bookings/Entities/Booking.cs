using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Domain.Shared.Exceptions;

namespace Bookings.Domain.Bookings.Entities;

public class Booking
{
    public BookRef BookRef { get; }
    public DateTime BookDate { get; }
    public decimal TotalAmount { get; private set; }
    public IReadOnlySet<string> TicketNumbers => _ticketNumbers;

    private readonly HashSet<string> _ticketNumbers;
    
    public Booking(BookRef bookRef, DateTime bookDate)
    {
        BookRef = bookRef;
        BookDate = bookDate;
        TotalAmount = 0;
        _ticketNumbers = [];
    }

    public void AddTicket(string ticketNo, decimal ticketCost)
    {
        if (_ticketNumbers.Add(ticketNo))
            TotalAmount += ticketCost;
        else
            throw new InvalidDomainOperationException(
                $"A {nameof(Booking)} with {nameof(BookRef)} \"{BookRef}\" " +
                $"already has a {nameof(Ticket)} with {nameof(ticketNo)} \"{ticketNo}\".");
    }
}

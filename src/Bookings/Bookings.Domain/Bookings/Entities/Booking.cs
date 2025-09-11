using Bookings.Domain.Bookings.Helpers;

namespace Bookings.Domain.Bookings.Entities;

public class Booking
{
    public string BookRef { get; }
    public DateTime BookDate { get; }
    public decimal TotalAmount { get; private set; }
    public IReadOnlySet<string> TicketNumbers => _ticketNumbers;

    private HashSet<string> _ticketNumbers;
    
    public Booking(string bookRef, DateTime bookDate)
    {
        IataCodeChecker.CheckOrThrow(bookRef, 6);
        BookRef = bookRef;
        BookDate = bookDate;
        TotalAmount = 0;
        _ticketNumbers = [];
    }

    public void AddTicket(string ticketNo, decimal ticketCost)
    {
        if (_ticketNumbers.Add(ticketNo))
            TotalAmount += ticketCost;
    }
}

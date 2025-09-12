using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class Ticket
{
    public TicketNo TicketNo { get; }
    public BookRef BookRef { get; }
    public string PassengerId { get; }
    public string PassengerName { get; }
    public ContactData ContactData { get; }

    public Ticket(TicketNo ticketNo, BookRef bookRef, string passengerId, string passengerName, ContactData contactData)
    {
        TicketNo = ticketNo;
        BookRef = bookRef;
        PassengerId = passengerId;
        PassengerName = passengerName;
        ContactData = contactData;
    }
}

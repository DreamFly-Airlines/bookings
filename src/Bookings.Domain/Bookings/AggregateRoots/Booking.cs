using System.ComponentModel;
using Bookings.Domain.Bookings.Abstractions;
using Bookings.Domain.Bookings.Entities;
using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.Events;
using Bookings.Domain.Bookings.Exceptions;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.AggregateRoots;

public class Booking : AggregateRoot<IDomainEvent>
{
    public BookRef BookRef { get; }
    public DateTime BookDate { get; }
    public decimal TotalAmount { get; private set; }
    public BookingStatus Status { get; private set; }

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
            Passenger Passenger)> passengersTicketsInfo)
    {
        BookRef = bookRef;
        BookDate = bookDate;
        TotalAmount = 0;
        _tickets = [];
        Status = BookingStatus.Pending; 
        foreach (var ticketInfo in passengersTicketsInfo)
        {
            var ticket = new Ticket(
                ticketInfo.TicketNo, 
                BookRef, 
                ticketInfo.Passenger.PassengerId, 
                ticketInfo.Passenger.PassengerName, 
                ticketInfo.Passenger.ContactData);
            AddTicketAndEvaluateTotalAmountOrThrow(ticket, ticketInfo.TicketCost);
            foreach (var flightId in flightsIdsForTicket)
                ticket.AddFlight(flightId, fareConditions, ticketInfo.TicketCost);
        }
        AddDomainEvent(new BookingCreated(BookRef));
    }

    public void MarkAsPaid()
    {
        if (Status is BookingStatus.Pending)
        {
            Status = BookingStatus.Paid;
            AddDomainEvent(new BookingPaid(BookRef));
        }
        else
        {
            var reason = Status switch
            {
                BookingStatus.Paid => "already paid",
                BookingStatus.Cancelled => "cancelled",
                _ => throw new ArgumentException($"Booking doesn't support {nameof(Status)} {Status}")
            };
            throw new InvalidDomainOperationException($"Cannot mark booking as paid because {reason}");
        }
    }

    public void Cancel()
    {
        if (Status is BookingStatus.Pending)
        {
            Status = BookingStatus.Cancelled;
            AddDomainEvent(new BookingCancelled(BookRef));
        }
        else
        {
            var reason = Status switch
            {
                BookingStatus.Paid => "paid",
                BookingStatus.Cancelled => "already cancelled",
                _ => throw new ArgumentException($"Booking doesn't support {nameof(Status)} {Status}")
            };
            throw new InvalidDomainOperationException($"Cannot cancel booking because {reason}");
        }
    }

    private void AddTicketAndEvaluateTotalAmountOrThrow(Ticket ticketNo, decimal ticketCost)
    {
        if (_tickets.Add(ticketNo))
            TotalAmount += ticketCost;
        else
            throw new InvalidDomainOperationException("Booking already has ticket with such ticket number");
    }
    
    // NOTE: consider removing this infrastructure detail
    [Obsolete("Used only by Entity Framework. Do not use it directly.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Booking() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}

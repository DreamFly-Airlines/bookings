using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.ReadModels;

public class BookingReadModel
{
    public required BookRef BookRef { get; init; }
    public required DateTime BookDate { get; init; }
    public required decimal TotalAmount { get; init; }
    public required BookingStatus Status { get; init; }
    public required List<TicketReadModel> Tickets { get; init; }
}
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Services;

public interface ITicketNoGeneratorService
{
    public TicketNo Generate();
}
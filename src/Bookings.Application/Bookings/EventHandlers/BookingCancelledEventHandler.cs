using Bookings.Domain.Bookings.Events;
using Shared.Abstractions.Events;

namespace Bookings.Application.Bookings.EventHandlers;

public class BookingCancelledEventHandler : IEventHandler<BookingCancelled>
{
    public Task HandleAsync(BookingCancelled @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
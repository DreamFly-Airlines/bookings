using Bookings.Application.Abstractions;

namespace Bookings.Application.Bookings.Commands;

public class BookFlightCommandHandler : ICommandHandler<BookFlightCommand>
{
    public async Task HandleAsync(BookFlightCommand command, CancellationToken cancellationToken = default)
    {
        var booking = new Bookings
    }
}
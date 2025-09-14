using Bookings.Application.Abstractions;
using Bookings.Application.Services;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Commands;

public class BookFlightCommandHandler(
    IStringBackedDataGeneratorService generator) : ICommandHandler<MakeBookingCommand>
{
    public async Task HandleAsync(MakeBookingCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        
    }
}
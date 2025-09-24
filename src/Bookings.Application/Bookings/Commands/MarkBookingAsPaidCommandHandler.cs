using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Exceptions;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Commands;

public class MarkBookingAsPaidCommandHandler(
    IBookingRepository bookingRepository) : ICommandHandler<MarkBookingAsPaidCommand>
{
    public async Task HandleAsync(MarkBookingAsPaidCommand command, CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetByBookRefAsync(command.BookRef, cancellationToken);
        if (booking is null)
            throw new NotFoundException(nameof(Booking),  command.BookRef, idName: nameof(BookRef));
        booking.MarkAsPaid();
        await  bookingRepository.SaveChangesAsync(booking, cancellationToken);
    }
}
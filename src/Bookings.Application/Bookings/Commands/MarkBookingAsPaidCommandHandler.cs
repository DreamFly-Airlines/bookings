using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Exceptions;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Exceptions;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Bookings.Application.Bookings.Commands;

public class MarkBookingAsPaidCommandHandler(
    ILogger<MarkBookingAsPaidCommandHandler> logger,
    IBookingRepository bookingRepository) : ICommandHandler<MarkBookingAsPaidCommand>
{
    public async Task HandleAsync(MarkBookingAsPaidCommand command, CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetByBookRefAsync(command.BookRef, cancellationToken);
        if (booking is null)
            throw new NotFoundException(nameof(Booking),  command.BookRef, idName: nameof(BookRef));
        try
        {
            booking.MarkAsPaid();
            await bookingRepository.SaveChangesAsync(booking, cancellationToken);
            logger.LogInformation(
                "{nameofBooking} with {nameofBookRef} \"{BookRef}\" paid",
                nameof(Booking), nameof(BookRef), booking.BookRef);
        }
        catch (InvalidDomainOperationException ex)
        {
            var state = new EntityStateInfo(nameof(Booking), 
                (nameof(Booking.BookRef), booking.BookRef),
                (nameof(Booking.Status), booking.Status.ToString()));
            throw new ClientValidationException(ex.Message, state);
        }
    }
}
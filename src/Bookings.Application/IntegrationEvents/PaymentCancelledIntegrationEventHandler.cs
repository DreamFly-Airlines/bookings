using Bookings.Application.Bookings.Commands;
using Bookings.Application.Bookings.Exceptions;
using Bookings.Domain.Bookings.Exceptions;
using Bookings.Domain.Bookings.ValueObjects;
using Shared.Abstractions.Commands;
using Shared.Abstractions.IntegrationEvents;
using Shared.IntegrationEvents.Payments;

namespace Bookings.Application.IntegrationEvents;

public class PaymentCancelledIntegrationEventHandler(
    ICommandSender commandSender) : IIntegrationEventHandler<PaymentCancelledIntegrationEvent>
{
    public async Task HandleAsync(PaymentCancelledIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bookRef = BookRef.FromString(integrationEvent.BookRef);
            var command = new CancelBookingCommand(bookRef);
            await commandSender.SendAsync(command, cancellationToken);
        }
        catch (InvalidDataFormatException ex)
        {
            var state = new EntityStateInfo(
                nameof(BookRef), ("Value", integrationEvent.BookRef));
            throw new ServerValidationException(ex.Message, state);
        }
    }
}
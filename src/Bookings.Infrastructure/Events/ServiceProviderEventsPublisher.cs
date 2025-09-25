using Bookings.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Infrastructure.Events;

public class ServiceProviderEventsPublisher(
    IServiceProvider serviceProvider) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event, cancellationToken);
        }
    }
}
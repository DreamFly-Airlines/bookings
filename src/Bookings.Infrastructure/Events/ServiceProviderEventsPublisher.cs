using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Events;

namespace Bookings.Infrastructure.Events;

public class ServiceProviderEventsPublisher(
    IServiceProvider serviceProvider) : IEventPublisher
{
    private const string HandleMethodName = nameof(IEventHandler<object>.HandleAsync);
    
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : notnull
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        var handleMethod = handlerType.GetMethod(HandleMethodName) 
                               ?? throw new ArgumentNullException(HandleMethodName);
        var handlers = serviceProvider.GetServices(handlerType);
        foreach (var handler in handlers)
            await (Task)handleMethod.Invoke(handler, [@event, cancellationToken])!;
    }
}
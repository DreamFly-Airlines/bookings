namespace Bookings.Application.Abstractions;

public interface IEventHandler<in TEvent>
{
    public Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
namespace Bookings.Application.Abstractions;

public interface ICommandHandler<in T> where T : ICommand
{
    public Task HandleAsync(T command, CancellationToken cancellationToken = default);
}
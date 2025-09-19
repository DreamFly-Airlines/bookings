namespace Bookings.Application.Abstractions;

public interface IQuerySender
{
    public Task<T> SendAsync<T>(IQuery<T> query, CancellationToken cancellationToken = default);
}
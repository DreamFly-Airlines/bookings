namespace Bookings.Application.Bookings.Abstractions;

public interface IQueriesSender
{
    public Task<T> SendAsync<T>(IQuery<T> query, CancellationToken cancellationToken = default);
}
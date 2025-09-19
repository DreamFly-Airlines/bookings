namespace Bookings.Application.Abstractions;

public interface IQueryHandler<in TQuery, T> where TQuery : IQuery<T>
{
    public Task<T> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
using Bookings.Application.Bookings.Abstractions;
using Bookings.Domain.Bookings.Entities;

namespace Bookings.Application.Bookings.Queries;

public class GetMatchingFlightsQueryHandler : IQueryHandler<GetMatchingFlightsQuery, IEnumerable<Flight>>
{
    public Task<IEnumerable<Flight>> HandleAsync(GetMatchingFlightsQuery query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
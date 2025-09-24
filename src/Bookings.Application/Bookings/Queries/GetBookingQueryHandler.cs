using Bookings.Application.Abstractions;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Repositories;

namespace Bookings.Application.Bookings.Queries;

public class GetBookingQueryHandler(
    IBookingRepository bookingRepository) : IQueryHandler<GetBookingQuery, Booking?>
{
    public async Task<Booking?> HandleAsync(GetBookingQuery query, CancellationToken cancellationToken = default)
    {
        return await bookingRepository.GetByBookRefAsync(query.BookRef, cancellationToken);
    }
}
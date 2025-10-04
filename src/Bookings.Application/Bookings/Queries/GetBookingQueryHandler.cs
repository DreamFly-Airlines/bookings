using Bookings.Application.Bookings.Exceptions;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;
using Shared.Abstractions.Queries;

namespace Bookings.Application.Bookings.Queries;

public class GetBookingQueryHandler(
    IBookingRepository bookingRepository) : IQueryHandler<GetBookingQuery, Booking>
{
    public async Task<Booking> HandleAsync(GetBookingQuery query, CancellationToken cancellationToken = default)
        => await bookingRepository.GetByBookRefAsync(query.BookRef, cancellationToken) 
               ?? throw new NotFoundException(nameof(Booking), query.BookRef, nameof(BookRef));
}
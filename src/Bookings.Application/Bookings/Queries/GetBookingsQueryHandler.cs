using Bookings.Application.Bookings.ReadModels;
using Bookings.Application.Bookings.Repositories;
using Shared.Abstractions.Queries;

namespace Bookings.Application.Bookings.Queries;

public class GetBookingsQueryHandler(
    IBookingReadModelRepository bookingReadModelRepository) : IQueryHandler<GetBookingsQuery, List<BookingReadModel>>
{
    public async Task<List<BookingReadModel>> HandleAsync(GetBookingsQuery query, CancellationToken cancellationToken = new CancellationToken())
    {
        return await bookingReadModelRepository.GetBookingsForUserAsync(query.UserId, cancellationToken);
    }
}
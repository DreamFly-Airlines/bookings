using Bookings.Application.Bookings.ReadModels;

namespace Bookings.Application.Bookings.Repositories;

public interface IBookingReadModelRepository
{
    public Task<List<BookingReadModel>> GetBookingsForUserAsync(
        string userId, CancellationToken cancellationToken = default);
}
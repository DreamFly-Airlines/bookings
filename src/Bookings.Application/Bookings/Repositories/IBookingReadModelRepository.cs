using Bookings.Application.Bookings.ReadModels;

namespace Bookings.Application.Bookings.Repositories;

public interface IBookingReadModelRepository
{
    public Task<List<BookingReadModel>> GetBookingsForPassengerAsync(
        string passengerId, CancellationToken cancellationToken = default);
}
using Bookings.Application.Bookings.ReadModels;
using Bookings.Application.Bookings.Repositories;
using Bookings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Infrastructure.Repositories;

public class SqlBookingReadModelRepository(BookingsDbContext dbContext) : IBookingReadModelRepository
{
    public async Task<List<BookingReadModel>> GetBookingsForPassengerAsync(string passengerId, CancellationToken cancellationToken = default)
    {
        return [];
    }
}
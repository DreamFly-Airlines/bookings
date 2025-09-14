using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Infrastructure.Repositories;

public class SqlBookingRepository(BookingsDbContext dbContext) : IBookingRepository
{
    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await dbContext.Bookings.AddAsync(booking, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Booking?> GetByBookRefAsync(BookRef bookRef, CancellationToken cancellationToken = default)
    {
        var booking = await dbContext.Bookings
            .FirstOrDefaultAsync(b => b.BookRef == bookRef, cancellationToken);
        return booking;
    }

    public async Task DeleteAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        dbContext.Bookings.Remove(booking);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
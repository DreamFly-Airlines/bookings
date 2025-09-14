using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Repositories;

public interface IBookingRepository
{
    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    
    public Task<Booking?> GetByBookRefAsync(BookRef bookRef, CancellationToken cancellationToken = default);
    
    public Task DeleteAsync(Booking booking, CancellationToken cancellationToken = default);
    
    public Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
}
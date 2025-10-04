using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Events;

namespace Bookings.Infrastructure.Repositories;

public class InMemoryBookingRepository(
    IServiceScopeFactory scopeFactory) : IBookingRepository
{
    private static readonly Dictionary<BookRef, Booking> Bookings = new();
    
    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        Bookings[booking.BookRef] = booking;
        await SaveChangesAsync(booking, cancellationToken);
    }

    public Task<Booking?> GetByBookRefAsync(BookRef bookRef, CancellationToken cancellationToken = default)
    {
        Bookings.TryGetValue(bookRef, out var booking);
        return Task.FromResult(booking);
    }

    public Task DeleteAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        Bookings.Remove(booking.BookRef);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        using var scope = scopeFactory.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();
        var events = booking.DomainEvents.ToArray();
        booking.ClearDomainEvents();
        foreach (var @event in events)
            await publisher.PublishAsync(@event, cancellationToken);
    }
}
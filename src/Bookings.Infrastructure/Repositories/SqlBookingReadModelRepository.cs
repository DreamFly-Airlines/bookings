using Bookings.Application.Bookings.ReadModels;
using Bookings.Application.Bookings.Repositories;
using Bookings.Domain.Bookings.Enums;
using Bookings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Infrastructure.Repositories;

public class SqlBookingReadModelRepository(BookingsDbContext dbContext) : IBookingReadModelRepository
{
    public async Task<List<BookingReadModel>> GetBookingsForUserAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        // TODO: it makes 2 queries to database, possibly it might be worth using clear sql or creating a view
        
        var bookings = await dbContext.Bookings
            .AsNoTracking()
            .Where(b => b.CreatorId == userId)
            .Include(b => b.Tickets)
            .ThenInclude(t => t.TicketFlights)
            .ToListAsync(cancellationToken);

        // TODO: Distinct() may be unnecessary because all tickets within a booking are the same
        // (at the moment, but this is not currently validated)
        var flightsIds = bookings
            .SelectMany(b => b.Tickets
                .SelectMany(t => t.TicketFlights
                    .Select(tf => tf.FlightId)))
            .ToHashSet();
        var flights = await dbContext.FlightsView
            .Where(f => flightsIds.Contains(f.FlightId))
            .ToDictionaryAsync(f => f.FlightId, f => f, cancellationToken);
        var bookingReadModels = bookings
            .Select(b => new BookingReadModel
            {
                BookRef = b.BookRef,
                BookDate = b.BookDate,
                TotalAmount = b.TotalAmount,
                Status = b.Status,
                Tickets = b.Tickets
                    .Select(t => new TicketReadModel
                    {
                        TicketNo = t.TicketNo,
                        PassengerName = t.PassengerName,
                        ContactData = t.ContactData,
                        TicketFlights = t.TicketFlights
                            .Select(tf => new TicketFlightReadModel
                            {
                                FareConditions = tf.FareConditions,
                                Amount = tf.Amount,
                                Flight = flights[tf.FlightId]
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .ToList();
        return bookingReadModels;
    }
}
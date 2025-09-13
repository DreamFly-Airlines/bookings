using System.Linq.Expressions;
using Bookings.Application.Bookings.ReadModels.ReadModels;
using Bookings.Application.Bookings.ReadModels.Repositories;
using Bookings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Infrastructure.Repositories;

public class SqlFlightReadModelRepository(BookingsDbContext dbContext) : IFlightReadModelRepository
{
    public async Task<List<FlightReadModel>> Where(
        Expression<Func<FlightReadModel, bool>> filter, CancellationToken cancellationToken = default)
        => await dbContext.FlightsView.Where(filter).ToListAsync(cancellationToken: cancellationToken);
}
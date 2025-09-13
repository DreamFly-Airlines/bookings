using System.Linq.Expressions;
using Bookings.Application.Bookings.ReadModels.ReadModels;

namespace Bookings.Application.Bookings.ReadModels.Repositories;

public interface IFlightReadModelRepository
{
    public Task<List<FlightReadModel>> Where(
        Expression<Func<FlightReadModel, bool>> filter, CancellationToken cancellationToken = default);
}
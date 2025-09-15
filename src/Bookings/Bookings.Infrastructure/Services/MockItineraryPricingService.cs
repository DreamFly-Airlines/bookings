using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.Enums;

namespace Bookings.Infrastructure.Services;

public class MockItineraryPricingService :  IItineraryPricingService
{
    // TODO: create pricing microservice and use its API
    public Task<decimal> CalculatePriceAsync(IEnumerable<int> flightsIds, FareConditions fareConditions,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(1000m);
    }
}
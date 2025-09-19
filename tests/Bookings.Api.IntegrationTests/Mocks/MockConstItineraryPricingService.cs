using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.Enums;

namespace Bookings.Api.IntegrationTests.Mocks;

public class MockConstItineraryPricingService : IItineraryPricingService
{
    public const decimal MockPrice = 1000;
    
    public Task<decimal> CalculatePriceAsync(IEnumerable<int> flightsIds, FareConditions fareConditions,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(MockPrice);
    }
}
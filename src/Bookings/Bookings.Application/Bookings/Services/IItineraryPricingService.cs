using Bookings.Domain.Bookings.Enums;

namespace Bookings.Application.Bookings.Services;

public interface IItineraryPricingService
{
    public Task<decimal> CalculatePriceAsync(
        IEnumerable<int> flightsIds, FareConditions fareConditions, CancellationToken cancellationToken = default);
}
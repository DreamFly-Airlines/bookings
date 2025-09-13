using Bookings.Application.Bookings.ReadModels.ReadModels;

namespace Bookings.Application.Services;

public interface IFlightSearchingService
{
    public Task<List<FlightReadModel>> SearchAsync(
        string departureCity,
        string arrivalCity,
        DateOnly departureDate,
        int passengersCount,
        CancellationToken cancellationToken = default);
}
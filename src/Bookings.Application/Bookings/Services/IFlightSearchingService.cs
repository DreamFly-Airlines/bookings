using Bookings.Application.Bookings.ReadModels;

namespace Bookings.Application.Bookings.Services;

public interface IFlightSearchingService
{
    // TODO: search flights with layovers 
    public Task<List<FlightReadModel>> SearchAsync(
        string departureCity,
        string arrivalCity,
        DateOnly departureDate,
        int passengersCount,
        CancellationToken cancellationToken = default);
}
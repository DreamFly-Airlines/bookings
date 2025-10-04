using Bookings.Application.Bookings.ReadModels.ReadModels;
using Bookings.Application.Bookings.Services;
using Shared.Abstractions.Queries;

namespace Bookings.Application.Bookings.Queries;

public class SearchFlightsItineraryQueryQueryHandler(
    IFlightSearchingService flightSearchingService) 
    : IQueryHandler<SearchFlightsItineraryQuery, List<FlightReadModel>>
{
    public async Task<List<FlightReadModel>> HandleAsync(
        SearchFlightsItineraryQuery query, CancellationToken cancellationToken = default)
    {
        return await flightSearchingService.SearchAsync(
            query.DepartureCity,
            query.ArrivalCity,
            query.DepartureDate,
            query.PassengersCount,
            cancellationToken);
    }
}
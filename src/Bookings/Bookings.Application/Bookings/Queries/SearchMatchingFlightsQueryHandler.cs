using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.ReadModels.ReadModels;
using Bookings.Application.Services;
namespace Bookings.Application.Bookings.Queries;

public class SearchMatchingFlightsQueryHandler(
    IFlightSearchingService flightSearchingService) 
    : IQueryHandler<SearchMatchingFlightsQuery, List<FlightReadModel>>
{
    public async Task<List<FlightReadModel>> HandleAsync(
        SearchMatchingFlightsQuery query, CancellationToken cancellationToken = default)
    {
        return await flightSearchingService.SearchAsync(
            query.DepartureCity,
            query.ArrivalCity,
            query.DepartureDate,
            query.PassengersCount,
            cancellationToken);
    }
}
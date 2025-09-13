using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.ReadModels.ReadModels;
using Bookings.Application.Bookings.ReadModels.Repositories;
using Bookings.Domain.Bookings.Enums;
namespace Bookings.Application.Bookings.Queries;

public class GetMatchingFlightsQueryHandler(
    IFlightReadModelRepository flightReadModelRepository) 
    : IQueryHandler<GetMatchingFlightsQuery, List<FlightReadModel>>
{
    public async Task<List<FlightReadModel>> HandleAsync(
        GetMatchingFlightsQuery query, CancellationToken cancellationToken = default)
    {
        var flights = await flightReadModelRepository
            .Where(flight =>
                    flight.DepartureCity == query.DepartureCity &&
                    flight.ArrivalCity == query.ArrivalCity && 
                    ((flight.Status == FlightStatus.Scheduled || flight.Status == FlightStatus.OnTime) 
                        && flight.ScheduledDeparture.Date == query.DepartureDate
                     || 
                     (flight.Status == FlightStatus.Delayed 
                        && flight.ActualDeparture!.Value.Date == query.DepartureDate.Date)),
                cancellationToken);

        return flights;
    }
}
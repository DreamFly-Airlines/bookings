using Bookings.Application.Bookings.ReadModels.ReadModels;
using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.Enums;
using Bookings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Infrastructure;

public class SqlFlightSearchingService(BookingsDbContext dbContext) : IFlightSearchingService
{
    public async Task<List<FlightReadModel>> SearchAsync(
        string departureCity, 
        string arrivalCity, 
        DateOnly departureDate, 
        int passengersCount,
        CancellationToken cancellationToken = default)
    {
        var matchingFlights = dbContext.FlightsView.Where(flight =>
                flight.DepartureCity == departureCity &&
                flight.ArrivalCity == arrivalCity && 
                ((flight.Status == FlightStatus.Scheduled || flight.Status == FlightStatus.OnTime) 
                 && DateOnly.FromDateTime(flight.ScheduledDeparture) == departureDate
                 || 
                 (flight.Status == FlightStatus.Delayed 
                  && DateOnly.FromDateTime(flight.ActualDeparture!.Value) == departureDate)));
        var flightsWithSeatsInfo = matchingFlights.Select(flight => new
        {
            Flight = flight,
            TotalSeatsCount = dbContext.Aircrafts.Count(aircraft => aircraft.AircraftCode == flight.AircraftCode),
            BookedSeatsCount = dbContext.TicketFlights.Count(ticketFlight => ticketFlight.FlightId == flight.FlightId)
        });
        return await flightsWithSeatsInfo
            .Where(info => 
                info.TotalSeatsCount - info.BookedSeatsCount >= passengersCount)
            .Select(info => info.Flight)
            .ToListAsync(cancellationToken);
    }
}
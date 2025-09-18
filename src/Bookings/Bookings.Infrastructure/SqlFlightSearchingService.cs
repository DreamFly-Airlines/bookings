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
        var fromDate = departureDate.ToDateTime(TimeOnly.MinValue);
        var fromDateUtc = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
        var toDateUtc = fromDateUtc.AddDays(1);
        var matchingFlights = dbContext.FlightsView.Where(flight =>
            flight.DepartureCity == departureCity &&
            flight.ArrivalCity == arrivalCity &&
            ((flight.Status == FlightStatus.Scheduled || flight.Status == FlightStatus.OnTime)
             && flight.ScheduledDeparture >= fromDateUtc && flight.ScheduledDeparture < toDateUtc
             ||
             (flight.Status == FlightStatus.Delayed
              && flight.ActualDeparture! >= fromDateUtc &&  flight.ActualDeparture! < toDateUtc)));
        var matchingFlightsBySeats = matchingFlights.Select(flight => new
            {
                Flight = flight,
                TotalSeatsCount = dbContext.Aircrafts.Count(aircraft => aircraft.AircraftCode == flight.AircraftCode),
                BookedSeatsCount =
                    dbContext.TicketFlights.Count(ticketFlight => ticketFlight.FlightId == flight.FlightId)
            })
            .Where(info =>
                info.TotalSeatsCount - info.BookedSeatsCount >= passengersCount)
            .Select(info => info.Flight);
        Console.WriteLine(matchingFlightsBySeats.ToQueryString());
        return await matchingFlightsBySeats
            .ToListAsync(cancellationToken);
    }
}
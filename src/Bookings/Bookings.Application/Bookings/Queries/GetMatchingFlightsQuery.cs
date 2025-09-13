using Bookings.Application.Abstractions;
using Bookings.Domain.Bookings.Entities;

namespace Bookings.Application.Bookings.Queries;

public record GetMatchingFlightsQuery(
    string DepartureCity, 
    string ArrivalCity, 
    DateTime DepartureDate, 
    int PassengersCount) : IQuery<IEnumerable<Flight>>;
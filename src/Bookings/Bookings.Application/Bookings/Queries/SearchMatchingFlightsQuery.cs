using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.ReadModels.ReadModels;

namespace Bookings.Application.Bookings.Queries;

public record SearchMatchingFlightsQuery(
    string DepartureCity, 
    string ArrivalCity, 
    DateOnly DepartureDate, 
    int PassengersCount) : IQuery<List<FlightReadModel>>;
using Bookings.Application.Bookings.ReadModels;
using Shared.Abstractions.Queries;

namespace Bookings.Application.Bookings.Queries;

public record SearchFlightsItineraryQuery(
    string DepartureCity, 
    string ArrivalCity, 
    DateOnly DepartureDate, 
    int PassengersCount) : IQuery<List<FlightReadModel>>;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.ReadModels.ReadModels;

namespace Bookings.Application.Bookings.Queries;

public record SearchFlightsItineraryQuery(
    string DepartureCity, 
    string ArrivalCity, 
    DateOnly DepartureDate, 
    int PassengersCount) : IQuery<List<FlightReadModel>>;
using Bookings.Domain.Bookings.Helpers;

namespace Bookings.Domain.Bookings.Entities;

public class AircraftData
{
    public string AircraftCode { get; }
    public string Model { get; }
    public int Range { get; }
    public IReadOnlySet<(string FlightNo, DateTime DepartureDate)> FlightIds => _flightIds;
    public IReadOnlySet<Seat> Seats { get; }

    private readonly HashSet<(string FlightNo, DateTime DepartureDate)> _flightIds;

    public AircraftData(string aircraftCode, string model, int range, HashSet<Seat> seats)
    {
        IataCodeChecker.CheckOrThrow(aircraftCode, 3);
        AircraftCode = aircraftCode;
        Model = model;
        Range = range;
        Seats = seats;
        _flightIds = [];
    }

    public void RegisterFlight(string flightNo, DateTime departureDate)
    {
        if (!_flightIds.Add((flightNo, departureDate)))
            throw new InvalidOperationException(
                $"{nameof(Flight)} with {nameof(flightNo)} \"{flightNo}\" " +
                $"and {nameof(departureDate)} \"{departureDate}\" already added to {nameof(AircraftData)} " +
                $"for {nameof(Aircraft)} with {nameof(AircraftCode)} \"{AircraftCode}\".");
    }
}

using Bookings.Domain.Bookings.Helpers;

namespace Bookings.Domain.Bookings.Entities;

public class AircraftData
{
    public string AircraftCode { get; }
    public string Model { get; }
    public int Range { get; }
    public virtual ICollection<Flight> Flights { get; } = new List<Flight>();
    public virtual ICollection<Seat> Seats { get; } = new List<Seat>();

    public AircraftData(string aircraftCode, string model, int range)
    {
        IataCodeChecker.CheckOrThrow(aircraftCode, 3);
        AircraftCode = aircraftCode;
        Model = model;
        Range = range;
        
    }
}

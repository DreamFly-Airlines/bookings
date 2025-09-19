using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class Aircraft
{
    public AircraftCode AircraftCode { get; }
    public string Model { get; }
    public int Range { get; }

    public Aircraft(AircraftCode aircraftCode, string model, int range)
    {
        AircraftCode = aircraftCode;
        Model = model;
        Range = range;
    }
}

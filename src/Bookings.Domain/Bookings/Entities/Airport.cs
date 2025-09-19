using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class Airport
{
    public IataAirportCode AirportCode { get; }
    public string AirportName { get; }
    public string City { get; }
    public Coordinates Coordinates { get; }
    public IanaTimezone Timezone { get; }

    public Airport(IataAirportCode airportCode, string airportName, string city, Coordinates coordinates, IanaTimezone timezone)
    {
        AirportCode = airportCode;
        AirportName = airportName;
        City = city;
        Coordinates = coordinates;
        Timezone = timezone;
    }
}

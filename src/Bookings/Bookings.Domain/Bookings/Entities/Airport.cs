using Bookings.Domain.Bookings.Helpers;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class Airport
{
    public string AirportCode { get; }
    public string AirportName { get; }
    public string City { get; }
    public Coordinates Coordinates { get; }
    public string Timezone { get; }

    public Airport(string airportCode, string airportName, string city, Coordinates coordinates, string timezone)
    {
        CheckAirportCodeOrThrow(airportCode);
        IanaTimezoneChecker.CheckOrThrow(timezone);
        AirportCode = airportCode;
        AirportName = airportName;
        City = city;
        Coordinates = coordinates;
        Timezone = timezone;
    }

    private static void CheckAirportCodeOrThrow(string airportCode)
    {
        if (airportCode.Length != 3)
            throw new FormatException($"{nameof(airportCode)} should be exactly three characters");
        for (var i = 0; i < airportCode.Length; i++)
            if (airportCode[i] < 'A' && airportCode[i] > 'Z')
                throw new FormatException(
                    $"{nameof(airportCode)} must consist only of letters A-Z. " +
                    $"Unexpected character {airportCode[i]} at position {i}.");
    }
}

using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class Route
{
    public FlightNo FlightNo { get; }
    public IataAirportCode DepartureAirport { get; }
    public string DepartureAirportName { get; }
    public string DepartureCity { get; }
    public IataAirportCode ArrivalAirport { get; }
    public string ArrivalAirportName { get; }
    public string ArrivalCity { get; }
    public AircraftCode AircraftCode { get; }
    public TimeSpan? Duration { get; }
    public List<WeekDay>? DaysOfWeek { get; }
    
    public Route(
        FlightNo flightNo, 
        IataAirportCode departureAirport, 
        string departureAirportName, 
        string departureCity, 
        IataAirportCode arrivalAirport, 
        string arrivalAirportName, 
        string arrivalCity, 
        AircraftCode aircraftCode, 
        TimeSpan? duration, 
        List<WeekDay>? daysOfWeek)
    {
        FlightNo = flightNo;
        DepartureAirport = departureAirport;
        DepartureAirportName = departureAirportName;
        DepartureCity = departureCity;
        ArrivalAirport = arrivalAirport;
        ArrivalAirportName = arrivalAirportName;
        ArrivalCity = arrivalCity;
        AircraftCode = aircraftCode;
        Duration = duration;
        DaysOfWeek = daysOfWeek;
    }
}

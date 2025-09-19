using Bookings.Domain.Bookings.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class FlightStatusConverter() : ValueConverter<FlightStatus, string>(
    enumStatus => ConvertToString(enumStatus), 
    stringStatus => ConvertToFlightStatus(stringStatus))
{
    private const string OnTimeName = "On time";
    private const string ArrivedName = "Arrived";
    private const string DepartedName = "Departed";
    private const string ScheduledName = "Scheduled";
    private const string DelayedName = "Delayed";
    private const string CancelledName = "Cancelled";
    

    private static string ConvertToString(FlightStatus flightStatus) => flightStatus switch
    {
        FlightStatus.Scheduled => ScheduledName,
        FlightStatus.Arrived => ArrivedName,
        FlightStatus.Departed => DepartedName,
        FlightStatus.Cancelled => CancelledName,
        FlightStatus.Delayed => DelayedName,
        FlightStatus.OnTime => OnTimeName,
        _ => throw new ArgumentOutOfRangeException(nameof(flightStatus), flightStatus, null)
    };
    
    private static FlightStatus ConvertToFlightStatus(string flightStatus) => flightStatus switch
    {
        ScheduledName => FlightStatus.Scheduled,
        ArrivedName => FlightStatus.Arrived,
        DelayedName => FlightStatus.Delayed, 
        DepartedName => FlightStatus.Departed,
        CancelledName => FlightStatus.Cancelled,
        OnTimeName => FlightStatus.OnTime,
        _ => throw new ArgumentOutOfRangeException(nameof(flightStatus), flightStatus, null)
    };
}
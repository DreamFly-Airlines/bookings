using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.Exceptions;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class Flight
{
    public int FlightId { get; }
    public FlightNo FlightNo { get; }
    public DateTime ScheduledDeparture { get; }
    public DateTime ScheduledArrival { get; }
    public IataAirportCode DepartureAirport { get; }
    public IataAirportCode ArrivalAirport { get; }
    public FlightStatus Status { get; private set; }
    public AircraftCode AircraftCode { get; }
    public DateTime? ActualDeparture { get; set; }
    public DateTime? ActualArrival { get; }

    public Flight(
        int flightId, 
        FlightNo flightNo, 
        DateTime scheduledDeparture, 
        DateTime scheduledArrival, 
        IataAirportCode departureAirport,
        IataAirportCode arrivalAirport,
        AircraftCode aircraftCode)
    {
        FlightId = flightId;
        FlightNo = flightNo;
        ScheduledDeparture = scheduledDeparture;
        ScheduledArrival = scheduledArrival;
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        AircraftCode = aircraftCode;
        Status = FlightStatus.Scheduled;
        ActualDeparture = null;
        ActualArrival = null;
    }

    public void MarkAsArrived(DateTime arrivalTime)
    {
        SetStatusOrThrow(FlightStatus.Arrived, 
            "mark flight as arrived",
            FlightStatus.Cancelled, FlightStatus.Arrived, FlightStatus.Departed);
        ActualDeparture = arrivalTime;
    }

    public void MarkAsDeparted(DateTime departureTime)
    {
        SetStatusOrThrow(FlightStatus.Departed, 
            "mark flight as departed",
            FlightStatus.Cancelled, FlightStatus.Arrived, FlightStatus.Departed);
        ActualDeparture = departureTime;
    }
    
    public void Delay()
    {
        SetStatusOrThrow(FlightStatus.Delayed, 
            "delay flight",
            FlightStatus.Cancelled, FlightStatus.Arrived, FlightStatus.Departed);
    }

    public void OpenForRegister(bool isDelayed)
    {
        if (Status is not FlightStatus.Scheduled)
            throw new InvalidDomainOperationException(
                "Cannot open flight for register because it's not scheduled");
        Status = isDelayed ? FlightStatus.Delayed : FlightStatus.OnTime;
    }

    private void SetStatusOrThrow(FlightStatus status, 
        string settingAction,
        params IEnumerable<FlightStatus> conflictingStatuses)
    {
        if (conflictingStatuses.Contains(Status))
            throw new InvalidDomainOperationException(
                $"Cannot {settingAction} because it's {GetHumanReadableStatus()}");
        Status = status;
    }

    private string GetHumanReadableStatus() => Status switch
    {
        FlightStatus.Scheduled => "scheduled",
        FlightStatus.OnTime => "on time",
        FlightStatus.Delayed => "delayed",
        FlightStatus.Departed => "departed",
        FlightStatus.Arrived => "arriver",
        FlightStatus.Cancelled => "cancelled",
        _ => throw new ArgumentOutOfRangeException(nameof(Status), Status.ToString())
    };
}

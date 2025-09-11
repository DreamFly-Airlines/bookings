using System;
using System.Collections.Generic;

namespace Bookings.Infrastructure.Persistence.Models;

/// <summary>
/// Flights
/// </summary>
public partial class Flight
{
    /// <summary>
    /// Flight ID
    /// </summary>
    public int FlightId { get; set; }

    /// <summary>
    /// Flight number
    /// </summary>
    public string FlightNo { get; set; } = null!;

    /// <summary>
    /// Scheduled departure time
    /// </summary>
    public DateTime ScheduledDeparture { get; set; }

    /// <summary>
    /// Scheduled arrival time
    /// </summary>
    public DateTime ScheduledArrival { get; set; }

    /// <summary>
    /// Airport of departure
    /// </summary>
    public string DepartureAirport { get; set; } = null!;

    /// <summary>
    /// Airport of arrival
    /// </summary>
    public string ArrivalAirport { get; set; } = null!;

    /// <summary>
    /// Flight status
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    /// Aircraft code, IATA
    /// </summary>
    public string AircraftCode { get; set; } = null!;

    /// <summary>
    /// Actual departure time
    /// </summary>
    public DateTime? ActualDeparture { get; set; }

    /// <summary>
    /// Actual arrival time
    /// </summary>
    public DateTime? ActualArrival { get; set; }

    public virtual AircraftsDatum AircraftCodeNavigation { get; set; } = null!;

    public virtual AirportsDatum ArrivalAirportNavigation { get; set; } = null!;

    public virtual AirportsDatum DepartureAirportNavigation { get; set; } = null!;

    public virtual ICollection<TicketFlight> TicketFlights { get; set; } = new List<TicketFlight>();
}

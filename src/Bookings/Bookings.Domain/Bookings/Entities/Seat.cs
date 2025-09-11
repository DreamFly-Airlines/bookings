namespace Bookings.Domain.Bookings.Entities;

public class Seat
{
    public string AircraftCode { get; set; } = null!;
    public string SeatNo { get; set; } = null!;
    public string FareConditions { get; set; } = null!;
    public virtual AircraftData AircraftCodeNavigation { get; set; } = null!;
}

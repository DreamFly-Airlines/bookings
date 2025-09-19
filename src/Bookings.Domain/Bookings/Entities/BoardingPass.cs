using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public class BoardingPass
{
    public TicketNo TicketNo { get; }
    public int FlightId { get; }
    public int BoardingNo { get; }
    public string SeatNo { get; }

    public BoardingPass(TicketNo ticketNo, int flightId, int boardingNo, string seatNo)
    {
        TicketNo = ticketNo;
        FlightId = flightId;
        BoardingNo = boardingNo;
        SeatNo = seatNo;
    }
}

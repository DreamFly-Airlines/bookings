using Bookings.Domain.Bookings.Exceptions;

namespace Bookings.Domain.Bookings.Entities;

public class Booking
{
    public string BookRef { get; }
    public DateTime BookDate { get; }
    public decimal TotalAmount { get; }
    public IReadOnlyList<Ticket> Tickets { get; } = new List<Ticket>();

    public Booking(string bookRef, DateTime bookDate, decimal totalAmount, IReadOnlyList<Ticket> tickets)
    {
        CheckBookRefFormatOrThrow(bookRef);
        BookRef = bookRef;
        BookDate = bookDate;
        TotalAmount = totalAmount;
    }

    private static void CheckBookRefFormatOrThrow(string bookRef)
    {
        if (bookRef.Length != 6)
            throw new InvalidIdFormat($"{nameof(BookRef)} must have 6 digits.");
        for (var i = 0; i < 6; i++)
            if (!char.IsDigit(bookRef[i]) && (bookRef[i] > 'Z' || bookRef[i] < 'A'))
                throw new InvalidIdFormat($"{nameof(BookRef)} must consist only of digits and letters A-Z. " +
                                          $"Unexpected character: {bookRef[i]}");
    }
}

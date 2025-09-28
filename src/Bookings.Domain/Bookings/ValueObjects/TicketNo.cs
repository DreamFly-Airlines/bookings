using Bookings.Domain.Bookings.Abstractions;
using Bookings.Domain.Bookings.Exceptions;

namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct TicketNo : IStringBackedData<TicketNo>
{
    public const int TicketNoLength = 13;
    private readonly string _value;
    
    private TicketNo(string value) => _value = value;
    
    public static TicketNo FromString(string @string)
    {
        if (@string.Length != TicketNoLength)
            throw new InvalidDataFormatException(
                $"Ticker number should consist of exactly {TicketNoLength} characters.");
        for (var i = 0; i < @string.Length; i++)
            if (!char.IsDigit(@string[i]))
                throw new InvalidDataFormatException(
                    $"Ticket number must consist only of numbers. " +
                    $"Unexpected character {@string[i]} at position {i}.");
        return new(@string);
    }
    
    public static implicit operator string(TicketNo ticketNo) => ticketNo._value;
    
    public override string ToString() => _value;
}
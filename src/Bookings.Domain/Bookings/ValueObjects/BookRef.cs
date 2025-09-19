using Bookings.Domain.Shared.Abstractions;

namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct BookRef : IStringBackedData<BookRef>
{
    public const int BookRefLength = 6;
    private readonly string _value;
    
    private BookRef(string value) => _value = value;
    
    public static BookRef FromString(string @string)
    {
        if (@string.Length != BookRefLength)
            throw new FormatException($"{nameof(@string)} must have {BookRefLength} digits.");
        for (var i = 0; i < BookRefLength; i++)
            if (!char.IsDigit(@string[i]) && (@string[i] > 'Z' || @string[i] < 'A'))
                throw new FormatException($"{nameof(@string)} must consist only of digits and letters A-Z. " +
                                          $"Unexpected character {@string[i]} at position {i}.");
        return new(@string);
    }
    
    public static implicit operator string(BookRef bookRef) => bookRef._value;
    
    public override string ToString() => _value;
}
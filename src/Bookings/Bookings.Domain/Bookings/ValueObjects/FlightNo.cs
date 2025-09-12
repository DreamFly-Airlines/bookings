namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct FlightNo
{
    private const int FlightNoLength = 6;
    private readonly string _value;
    
    private FlightNo(string value) => _value = value;
    
    public static FlightNo FromString(string @string)
    {
        if (@string.Length != FlightNoLength)
            throw new FormatException($"{nameof(@string)} should be exactly {FlightNoLength} characters");
        for (var i = 0; i < @string.Length; i++)
            if (!char.IsDigit(@string[i]) && (@string[i] < 'A' || @string[i] > 'Z'))
                throw new FormatException(
                    $"{nameof(@string)} must consist only of numbers and letters A-Z. " +
                    $"Unexpected character {@string[i]} at position {i}.");
        return new(@string);
    }
    
    public static implicit operator string(FlightNo flightNo) => flightNo._value;
    
    public override string ToString() => _value;
}
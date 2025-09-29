using Bookings.Domain.Bookings.Abstractions;
using Bookings.Domain.Bookings.Exceptions;

namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct IataAirportCode : IStringBackedData<IataAirportCode>
{
    private const int IataAirportCodeLength = 3; 
    private readonly string _value;
    
    private IataAirportCode(string value) => _value = value;
    
    public static IataAirportCode FromString(string @string)
    {
        if (@string.Length != IataAirportCodeLength)
            throw new InvalidDataFormatException(
                $"Airport code in IATA format should consist of exactly {IataAirportCodeLength} characters");
        for (var i = 0; i < @string.Length; i++)
            if (@string[i] < 'A' && @string[i] > 'Z')
                throw new InvalidDataFormatException(
                    $"Airport code in IATA format must consist only of letters A-Z. " +
                    $"Unexpected character {@string[i]} at position {i}");
        return new(@string);
    }
    
    public static implicit operator string(IataAirportCode timezone) => timezone._value;
    
    public override string ToString() => _value;
}
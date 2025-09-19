using Bookings.Domain.Shared.Abstractions;

namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct AircraftCode : IStringBackedData<AircraftCode>
{
    public const int AircraftCodeLength = 3;
    private readonly string _value;
    
    private AircraftCode(string value) => _value = value;

    public static AircraftCode FromString(string @string)
    {
        if (@string.Length != 3)
            throw new FormatException($"{nameof(AircraftCode)} should contain exactly {AircraftCodeLength} characters");
        for (var i = 0; i < @string.Length; i++)
            if (!char.IsDigit(@string[i]) && (@string[i] < 'A' || @string[i] > 'Z'))
                throw new FormatException($"{nameof(AircraftCode)} must consist only of numbers and letters A-Z). " +
                                          $"Unexpected character \'{@string[i]}' at position {i}.");
        return new(@string);
    }
    
    public static implicit operator string(AircraftCode aircraftCode) => aircraftCode._value;
    
    public override string ToString() => _value;
}
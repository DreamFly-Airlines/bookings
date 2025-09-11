namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct IanaTimezone
{
    private readonly string _value;
    
    private IanaTimezone(string value) => _value = value;
    
    public static IanaTimezone FromString(string @string)
    {
        var regionAndCity = @string.Split('/');
        if (regionAndCity.Length != 2)
            throw new FormatException(
                "The time zone in IANA format must consist of a region and a city, " +
                "separated by a slash (e.g., 'Europe/Moscow'). No slash found.");
        var (region, city) = (regionAndCity[0], regionAndCity[1]);
        CheckCapitalizedOrThrow(region, nameof(region));
        CheckCapitalizedOrThrow(city, nameof(city));
        return new(@string);
    }
    
    public static implicit operator string(IanaTimezone timezone) => timezone._value;
    
    public override string ToString() => _value;

    private static void CheckCapitalizedOrThrow(string @string, string paramName)
    {
        CheckHasAtLeastTwoLettersOrThrow(@string, paramName);
        if (@string[0] < 'A' || @string[0] > 'Z')
            throw new FormatException($"{paramName} should start with a capital letter.");
        for (var i = 1; i < @string.Length; i++)
            if (@string[i] < 'a' || @string[i] > 'z')
                throw new FormatException(
                    $"Except for the first uppercase letter, {paramName} must contain only lowercase letters. " +
                    $"Unexpected character {@string[i]} at position {i}.");
    }

    private static void CheckHasAtLeastTwoLettersOrThrow(string @string, string paramName)
    {
        if (@string.Length < 2)
            throw new FormatException($"{paramName} should contain at least 2 letters.");
    }
}
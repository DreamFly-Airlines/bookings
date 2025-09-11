using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class StringBackedStructConverter<T>(Func<string, T> fromString) : ValueConverter<T, string>(
    data => ConvertToStringOrThrow(data), 
    @string => fromString(@string)) 
    where T : struct
{
    private static string ConvertToStringOrThrow(T data)
    {
        var toStringValue = data.ToString();
        if (toStringValue is not null)
            return toStringValue;
        throw new InvalidCastException(
            $"Cannot convert {typeof(T).Name} to {typeof(string)}: the {nameof(ToString)}() method returns null.");
    }
}
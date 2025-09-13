using Bookings.Domain.Shared.Abstractions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class StringBackedDataConverter<T>() : ValueConverter<T, string>(
    data => ConvertToStringOrThrow(data), 
    @string => FromString(@string)) 
    where T : struct, IStringBackedData<T>
{
    private static string ConvertToStringOrThrow(T data)
    {
        var toStringValue = data.ToString();
        if (toStringValue is not null)
            return toStringValue;
        throw new InvalidCastException(
            $"Cannot convert {typeof(T).Name} to {typeof(string)}: the {nameof(ToString)}() method returns null.");
    }

    private static T FromString(string @string) => T.FromString(@string);
}
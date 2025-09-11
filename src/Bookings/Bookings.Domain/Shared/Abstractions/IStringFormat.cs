namespace Bookings.Domain.Shared.Abstractions;

public interface IStringFormat<out T>
{
    public static abstract T FromString(string @string);
}
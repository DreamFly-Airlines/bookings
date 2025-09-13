namespace Bookings.Domain.Shared.Abstractions;

// TODO: consider using code generator to avoid writing ToString(), implicit operator string(), etc. everytime
public interface IStringBackedData<out TSelf> where TSelf : struct, IStringBackedData<TSelf>
{
    public static abstract TSelf FromString(string @string);
}
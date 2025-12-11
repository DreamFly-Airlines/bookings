namespace Bookings.Application.Bookings.Configuration;

public record BookingOptions
{
    public TimeSpan ExpiresIn { get; init; } = TimeSpan.FromMinutes(5);
}
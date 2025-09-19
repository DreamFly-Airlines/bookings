namespace Bookings.Application.Bookings.Services;

public interface IClockService
{
    public Task<DateTime> NowAsync(CancellationToken cancellationToken = default);
}
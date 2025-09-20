using System.Data;
using Bookings.Application.Bookings.Services;
using Bookings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Infrastructure.Services;

// TODO: think about performance
public class SqlClockService(BookingsDbContext bookingsDbContext) : IClockService
{
    private const string BookingsNowFunction = "bookings.now()";
    
    public async Task<DateTime> NowAsync(CancellationToken cancellationToken = default)
    {
        var connection = bookingsDbContext.Database.GetDbConnection();
        var wasClosed = connection.State == ConnectionState.Closed;

        if (wasClosed)
            await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = $"select {BookingsNowFunction};";
        var funcResult = await command.ExecuteScalarAsync(cancellationToken);
        if (funcResult is null)
            throw new NullReferenceException($"{BookingsNowFunction} returned null.");
        
        if (wasClosed)
            await connection.CloseAsync();
        return (DateTime)funcResult;
    }
}
namespace Bookings.Application.Services;

public interface IBookingService
{
    public Task Book(string flightId, HashSet<string> userDocumentsIds);
}
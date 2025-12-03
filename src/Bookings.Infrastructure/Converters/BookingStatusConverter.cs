using Bookings.Domain.Bookings.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class BookingStatusConverter() : ValueConverter<BookingStatus, string>(
    enumStatus => ConvertFromBookingStatus(enumStatus),
    stringStatus => ConvertFromString(stringStatus))
{
    private const string PaidName = "Paid";
    private const string CancelledName = "Cancelled";
    private const string PendingName = "Pending";

    private static BookingStatus ConvertFromString(string bookingStatus) => bookingStatus switch
    {
        PaidName => BookingStatus.Paid,
        CancelledName => BookingStatus.Cancelled,
        PendingName => BookingStatus.Pending,
        _ => throw new ArgumentOutOfRangeException(nameof(bookingStatus))
    };

    private static string ConvertFromBookingStatus(BookingStatus bookingStatus) => bookingStatus switch
    {
        BookingStatus.Paid => PaidName,
        BookingStatus.Cancelled => CancelledName,
        BookingStatus.Pending => PendingName,
        _ => throw new ArgumentOutOfRangeException(nameof(bookingStatus))
    };
}
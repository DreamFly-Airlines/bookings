using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class IanaTimeZoneValueConverter() : ValueConverter<IanaTimezone, string>(
    timezone => timezone,
    @string => IanaTimezone.FromString(@string));
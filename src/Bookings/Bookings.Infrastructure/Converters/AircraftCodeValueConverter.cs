using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class AircraftCodeValueConverter() : ValueConverter<AircraftCode, string>(
    code => code,
    @string => AircraftCode.FromString(@string));
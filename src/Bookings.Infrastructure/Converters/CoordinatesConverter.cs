using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;

namespace Bookings.Infrastructure.Converters;

public class CoordinatesConverter() : ValueConverter<Coordinates, NpgsqlPoint>(
    coordinates => new NpgsqlPoint(coordinates.Longitude, coordinates.Latitude),
    point => new Coordinates(point.X, point.Y));
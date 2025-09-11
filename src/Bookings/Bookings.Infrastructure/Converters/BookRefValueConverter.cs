using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class BookRefValueConverter() : ValueConverter<BookRef, string>(
    bookRef => bookRef,
    @string => BookRef.FromString(@string));
using Bookings.Application.Abstractions;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Queries;

public record GetBookingQuery(BookRef BookRef) : IQuery<Booking?>;
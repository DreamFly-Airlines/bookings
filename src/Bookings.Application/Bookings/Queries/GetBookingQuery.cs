using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.ValueObjects;
using Shared.Abstractions.Queries;

namespace Bookings.Application.Bookings.Queries;

public record GetBookingQuery(BookRef BookRef) : IQuery<Booking>;
using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Domain.Shared.Abstractions;

namespace Bookings.Domain.Bookings.Events;

public record BookingMade(BookRef BookRef) : IDomainEvent;
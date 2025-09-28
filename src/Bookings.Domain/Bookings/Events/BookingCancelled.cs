using Bookings.Domain.Bookings.Abstractions;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Events;

public record BookingCancelled(BookRef BookRef) : IDomainEvent;
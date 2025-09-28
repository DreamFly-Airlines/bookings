using Bookings.Domain.Bookings.Abstractions;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Events;

public record BookingCreated(BookRef BookRef) : IDomainEvent;
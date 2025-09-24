using Bookings.Application.Abstractions;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Commands;

public record CancelBookingCommand(BookRef BookRef) : ICommand;
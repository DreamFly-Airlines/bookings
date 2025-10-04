using Bookings.Domain.Bookings.ValueObjects;
using Shared.Abstractions.Commands;

namespace Bookings.Application.Bookings.Commands;

public record CancelBookingCommand(BookRef BookRef) : ICommand;
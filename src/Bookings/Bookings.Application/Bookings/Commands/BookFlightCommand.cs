using Bookings.Application.Abstractions;

namespace Bookings.Application.Bookings.Commands;

public record BookFlightCommand(string FlightId, HashSet<string> UserDocumentsIds) : ICommand;
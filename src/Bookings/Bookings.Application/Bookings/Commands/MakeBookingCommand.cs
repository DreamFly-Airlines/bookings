using Bookings.Application.Abstractions;
using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Commands;

public record MakeBookingCommand(
    IEnumerable<int> FlightsIds, 
    ISet<(string PassengerId, string PassengerName, ContactData ContactData)> PassengersInfos,
    FareConditions FareConditions) : ICommand;
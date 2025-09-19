using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Dto;
using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Commands;

public record MakeBookingCommand(
    IEnumerable<int> ItineraryFlightsIds, 
    ISet<PassengerInfoDto> PassengersInfos,
    FareConditions FareConditions) : ICommand;
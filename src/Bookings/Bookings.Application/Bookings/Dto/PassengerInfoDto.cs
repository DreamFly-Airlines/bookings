using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Dto;

public readonly record struct PassengerInfoDto(string PassengerId, string PassengerName, ContactData ContactData);
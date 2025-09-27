namespace Bookings.Api.Dto;

public readonly record struct PassengerInfoDto(string PassengerId, string PassengerName, ContactDataDto ContactDataDto);
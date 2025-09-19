using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Domain.Bookings.Entities;

public record Passenger(string PassengerId, string PassengerName, ContactData ContactData);
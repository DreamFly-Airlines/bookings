namespace Bookings.Domain.Bookings.Exceptions;

public class InvalidDomainOperationException(string message) : Exception(message);
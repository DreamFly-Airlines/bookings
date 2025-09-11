namespace Bookings.Domain.Shared.Exceptions;

public class InvalidDomainOperationException(string message) : Exception(message);
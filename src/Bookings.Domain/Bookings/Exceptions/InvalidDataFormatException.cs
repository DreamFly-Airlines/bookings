namespace Bookings.Domain.Bookings.Exceptions;

public class InvalidDataFormatException(string message) : Exception(message);
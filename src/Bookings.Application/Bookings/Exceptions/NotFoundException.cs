namespace Bookings.Application.Bookings.Exceptions;

public class NotFoundException(
    string className, 
    string objectId, 
    string? idName) : Exception($"{className} with {idName ?? "ID"} \"{objectId}\" not found.");
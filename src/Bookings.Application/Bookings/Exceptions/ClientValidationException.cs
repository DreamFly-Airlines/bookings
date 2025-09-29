using Bookings.Application.Abstractions;

namespace Bookings.Application.Bookings.Exceptions;

public class ClientValidationException(string message, EntityStateInfo? entityStateInfo = null) 
    : BaseValidationException(message, entityStateInfo);
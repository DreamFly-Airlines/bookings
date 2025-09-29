using Bookings.Application.Bookings.Exceptions;

namespace Bookings.Application.Abstractions;

public abstract class BaseValidationException(string message, EntityStateInfo? entityStateInfo = null)
    : Exception(message)
{
    public EntityStateInfo? EntityStateInfo { get; } = entityStateInfo;
}
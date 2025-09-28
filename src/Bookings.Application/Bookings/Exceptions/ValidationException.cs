using System.Text;

namespace Bookings.Application.Bookings.Exceptions;

public class ValidationException(string message, bool isClientError, EntityStateInfo? entityStateInfo = null) : Exception(message)
{
    public EntityStateInfo? EntityStateInfo { get; } = entityStateInfo;
    public bool IsClientError { get; } = isClientError;
}
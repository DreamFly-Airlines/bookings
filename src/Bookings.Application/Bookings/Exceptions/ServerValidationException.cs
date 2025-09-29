using System.Text;
using Bookings.Application.Abstractions;

namespace Bookings.Application.Bookings.Exceptions;

public class ServerValidationException(string message, EntityStateInfo? entityStateInfo = null)
    : BaseValidationException(message, entityStateInfo);
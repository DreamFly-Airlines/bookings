using Bookings.Application.Bookings.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Api.ExceptionHandling;

public class GlobalExceptionHandler: IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _handlers;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
        _handlers = new()
        {
            { typeof(NotFoundException), HandleNotFoundAsync },
            { typeof(ValidationException), HandleValidationAsync },
            { typeof(Exception), HandleExceptionAsync }
        };
    }
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        var type = exception.GetType();
        if (!_handlers.TryGetValue(type, out var handleAsync)) 
            return false;
        
        await handleAsync(httpContext, exception);
        return true;
    }

    private async Task HandleAsync(
        HttpContext httpContext, 
        string errorType, 
        int statusCode, 
        string? messageForClient = null,
        string? additionalInfoForServer = null)
    {
        var additionalInfo = additionalInfoForServer is null ? string.Empty : $" {additionalInfoForServer}";
        var traceId = httpContext.TraceIdentifier;
        _logger.LogError(
            "Error for trace id \"{TraceId}\": {MessageForClient}{AdditionalInfo}", 
            traceId, messageForClient, additionalInfo);
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Detail = messageForClient,
            Status = statusCode,
            Instance = httpContext.Request.Path,
            Type = errorType
        });
    }
    
    private async Task HandleNotFoundAsync(HttpContext httpContext, Exception exception) 
        => await HandleAsync(
            httpContext, 
            "Not found", 
            StatusCodes.Status404NotFound,
            messageForClient: exception.Message);

    private async Task HandleValidationAsync(HttpContext httpContext, Exception exception)
    {
        var ex = (ValidationException)exception;
        if (ex.IsClientError)
            await HandleAsync(
                httpContext,
                "Validation error",
                StatusCodes.Status400BadRequest,
                messageForClient: exception.Message,
                additionalInfoForServer: ex.EntityStateInfo?.ToString());
        else
        {
            await HandleAsync(
                httpContext,
                "Internal server error",
                StatusCodes.Status500InternalServerError,
                additionalInfoForServer: $"{ex.Message} {ex.EntityStateInfo}");
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        => await HandleAsync(
            httpContext,
            "Internal server error", 
            StatusCodes.Status500InternalServerError,
            additionalInfoForServer: exception.Message);
}
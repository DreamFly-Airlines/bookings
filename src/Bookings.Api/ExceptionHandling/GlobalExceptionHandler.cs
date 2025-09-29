using Bookings.Application.Abstractions;
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
            { typeof(ServerValidationException), HandleServerValidationAsync },
            { typeof(ClientValidationException), HandleClientValidationAsync },
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
        string message,
        bool shouldIncludeMessageToClientResponse,
        string? additionalInfoForServer = null)
    {
        var traceId = httpContext.TraceIdentifier;
        var fullMessageForServer = additionalInfoForServer is not null
            ? $"{message}. Additional info: {additionalInfoForServer}"
            : message;
        _logger.LogError(
            "Error for trace id \"{traceId}\": {messageForServer}", 
            traceId, fullMessageForServer);
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Detail = shouldIncludeMessageToClientResponse ? null : message,
            Status = statusCode,
            Instance = httpContext.Request.Path,
            Type = errorType
        });
    }
    
    private async Task HandleValidationAsync(HttpContext httpContext, Exception exception, bool isClient)
    {
        var ex = (BaseValidationException)exception;
        await HandleAsync(
            httpContext,
            "Internal server error",
            StatusCodes.Status500InternalServerError,
            exception.Message,
            isClient,
            ex.EntityStateInfo?.ToString());
    }
    
    private async Task HandleNotFoundAsync(HttpContext httpContext, Exception exception) 
        => await HandleAsync(
            httpContext, 
            "Not found", 
            StatusCodes.Status404NotFound,
            message: exception.Message,
            true);

    private async Task HandleServerValidationAsync(HttpContext httpContext, Exception exception)
        => await HandleValidationAsync(httpContext, exception, false);
    
    private async Task HandleClientValidationAsync(HttpContext httpContext, Exception exception)
        => await HandleValidationAsync(httpContext, exception, true);
    
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        => await HandleAsync(
            httpContext,
            "Internal server error", 
            StatusCodes.Status500InternalServerError,
            exception.Message,
            false);
}
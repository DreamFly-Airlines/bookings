using Bookings.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Infrastructure.Queries;

public class ServiceProviderQuerySender(
    IServiceProvider serviceProvider) : IQuerySender
{
    public async Task<T> SendAsync<T>(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        var handleAsyncMethodName = nameof(IQueryHandler<IQuery<T>, T>.HandleAsync);
        var queryType = query.GetType();
        var queryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(T));
        var queryHandler = serviceProvider.GetRequiredService(queryHandlerType);
        var resultTaskObject = queryHandlerType
            .GetMethod(handleAsyncMethodName)?
            .Invoke(queryHandler, [query, cancellationToken]) 
                     ?? throw new MethodAccessException(
                         $"Cannot find or invoke method \"{handleAsyncMethodName}\" " +
                         $"of {queryHandlerType.Name}.");
        return await (Task<T>)resultTaskObject;
    }
}
using System.Reflection;
using Bookings.Application.Abstractions;

namespace Bookings.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryHandlers(
        this IServiceCollection services,
        Assembly assembly)
    {
        var handlerInterfaceType = typeof(IQueryHandler<,>);

        var types = assembly
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .Select(t => new
            {
                Implementation = t,
                Interfaces = t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
            });

        foreach (var type in types)
            foreach (var itf in type.Interfaces)
                services.AddScoped(itf, type.Implementation);

        return services;
    }
}
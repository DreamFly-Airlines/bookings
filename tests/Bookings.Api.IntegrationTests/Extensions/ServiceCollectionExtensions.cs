using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Api.IntegrationTests.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ReplaceSingletonService<TInterface, TImplementation>(this IServiceCollection serviceCollection) 
        where TInterface : class
        where TImplementation : class, TInterface
    {
        serviceCollection.RemoveServiceDescriptorIfExists<TInterface>();
        serviceCollection.AddSingleton<TInterface, TImplementation>();
    }

    public static void RemoveServiceDescriptorIfExists<T>(this IServiceCollection serviceCollection)
    {
        var descriptor = serviceCollection.SingleOrDefault(
            d => d.ServiceType == typeof(T));

        if (descriptor != null)
            serviceCollection.Remove(descriptor);
    }
}
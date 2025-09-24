using System.Reflection;
using Bookings.Application.Abstractions;
using Bookings.Infrastructure.Consumers;
using Confluent.Kafka;

namespace Bookings.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddQueryHandlers(
        this IServiceCollection services,
        Assembly assembly)
    {
        var handlerInterfaceType = typeof(IQueryHandler<,>);
        services.FindImplementationsAndRegister(handlerInterfaceType, assembly);
    }

    public static void AddCommandHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaceType = typeof(ICommandHandler<>);
        services.FindImplementationsAndRegister(handlerInterfaceType, assembly);
    }

    public static void AddKafkaConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        var consumerConfig = new ConsumerConfig();
        configuration.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
        
        services.AddTransient<IConsumer<Ignore, string>>(_ => 
            new ConsumerBuilder<Ignore, string>(consumerConfig).Build());

        services.AddHostedService<PaymentsEventsConsumer>();
    }

    private static void FindImplementationsAndRegister(this IServiceCollection services, Type interfaceType, Assembly assembly)
    {
        var types = assembly
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .Select(t => new
            {
                Implementation = t,
                Interfaces = t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
            });

        foreach (var type in types)
            foreach (var @interface in type.Interfaces)
                services.AddScoped(@interface, type.Implementation);
    }
}
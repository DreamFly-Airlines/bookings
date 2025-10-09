using Bookings.Infrastructure.Consumers;

namespace Bookings.Api.Extensions;

public static class ServiceCollectionExtensions
{

    public static void AddKafkaConsumers(this IServiceCollection services)
    {
        services.AddHostedService<PaymentsEventsConsumer>();
    }
}
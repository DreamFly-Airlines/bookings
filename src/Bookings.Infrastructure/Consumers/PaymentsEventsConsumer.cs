using System.Runtime.Serialization;
using System.Text.Json;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.ValueObjects;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Abstractions.IntegrationEvents;
using Shared.IntegrationEvents.Kafka;
using Shared.IntegrationEvents.Payments;

namespace Bookings.Infrastructure.Consumers;

public class PaymentsEventsConsumer : BackgroundService
{
    private readonly ILogger<PaymentsEventsConsumer> _logger;
    private readonly Lazy<IConsumer<Ignore, string>> _consumer;
    private readonly string _bootstrapServers;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Dictionary<string, Type> _eventTypeMap;

    public PaymentsEventsConsumer(
        ILogger<PaymentsEventsConsumer> logger,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        var consumerConfig = new ConsumerConfig();
        configuration.GetSection("Kafka:PaymentsEvents:ConsumerSettings").Bind(consumerConfig);
        _bootstrapServers = consumerConfig.BootstrapServers;
        _consumer = new(() => new ConsumerBuilder<Ignore, string>(consumerConfig).Build());
        _eventTypeMap = new()
        {
            { PaymentCancelledIntegrationEvent.EventName, typeof(PaymentCancelledIntegrationEvent) },
            { PaymentConfirmedIntegrationEvent.EventName, typeof(PaymentCancelledIntegrationEvent) }
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        if (!CheckBrokersAndLog(_bootstrapServers))
            return;
        _consumer.Value.Subscribe(KafkaTopics.PaymentsEvents);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Value.Consume(stoppingToken);
                var eventNameHeader = result.Message.Headers.FirstOrDefault(h => h.Key == KafkaHeaders.EventType);
                if (eventNameHeader is null)
                    throw new InvalidOperationException($"Missing header: {KafkaHeaders.EventType}");
                var eventName = System.Text.Encoding.UTF8.GetString(eventNameHeader.GetValueBytes())
                    ?? throw new InvalidOperationException($"Missing header value: {KafkaHeaders.EventType}");
                _logger.LogInformation(
                    $"Got event \"{eventNameHeader}\" for {nameof(Booking)} with {nameof(BookRef)} " +
                    $"\"{result.Message.Value}\".");
                using var scope = _scopeFactory.CreateScope();
                await HandleIntegrationEventAsyncOrThrow(
                    scope.ServiceProvider,
                    eventName,
                    result.Message.Value,
                    stoppingToken);
            }
            catch (ConsumeException e)
            {
                _logger.LogCritical("{ErrorMessage}", e.Message);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogCritical("{ErrorMessage}", e.Message);
            }
            catch (ArgumentException e)
            {
                _logger.LogCritical("{ErrorMessage}", e.Message);
            }
            catch (MissingMethodException e)
            {
                _logger.LogCritical("{ErrorMessage}", e.Message);
            }
            catch (SerializationException e)
            {
                _logger.LogCritical("{ErrorMessage}", e.Message);
            }
            catch (Exception e)
            {
                _logger.LogCritical("{ErrorMessage}", e.Message);
            }
        }
    }

    private async Task HandleIntegrationEventAsyncOrThrow(
        IServiceProvider serviceProvider, 
        string eventName, 
        string eventJson,
        CancellationToken cancellationToken = default)
    {
        var type = _eventTypeMap[eventName];
        var @event = JsonSerializer.Deserialize(eventJson, type) 
                     ?? throw new SerializationException(
                         $"Failed to deserialize event with {nameof(eventName)} \"{eventName}\": {eventJson}");
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(type);
        
        const string handleMethodName = nameof(IIntegrationEventHandler<PaymentCancelledIntegrationEvent>.HandleAsync);
        var handleMethodInfo = handlerType.GetMethod(handleMethodName)
                               ?? throw new MissingMethodException($"Method {handleMethodName} is missing");

        var atLeastOneHandlerFound = false;
        foreach (var handler in serviceProvider.GetServices(handlerType))
        {
            atLeastOneHandlerFound = true;
            await (Task)handleMethodInfo.Invoke(handler, [@event, cancellationToken])!;
        }

        if (!atLeastOneHandlerFound)
            throw new Exception($"No handlers for event with {nameof(eventName)} \"{eventName}\" were found");
    }

    private bool CheckBrokersAndLog(string bootstrapServers)
    {
        try
        {
            var config = new AdminClientConfig { BootstrapServers = bootstrapServers };
            using var admin = new AdminClientBuilder(config).Build();
            var metadata = admin.GetMetadata(TimeSpan.FromSeconds(2));
            if (metadata.Brokers.Count == 0)
            {
                _logger.LogWarning("Found 0 Kafka brokers");
                return false;
            }
            _logger.LogInformation("" +
                                   "Found {BrokersCount} Kafka brokers. " +
                                   "Ids: {BrokersIds}", 
                metadata.Brokers.Count, string.Join(", ", metadata.Brokers.Select(b => b.BrokerId)));
            return true;
        }
        catch (KafkaException ex)
        {
            _logger.LogWarning(
                "{KafkaException} thrown. " +
                "This is an expected behaviour if you are running without Docker/Kafka. " +
                "Details: {Details}", nameof(KafkaException), ex.Message);
            return false;
        }
    }
}
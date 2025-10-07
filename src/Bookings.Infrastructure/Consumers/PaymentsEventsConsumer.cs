using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Bookings.Application.Bookings.Commands;
using Bookings.Application.Payments.Enums;
using Bookings.Application.Payments.IntegrationEvents;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.ValueObjects;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Abstractions.Commands;

namespace Bookings.Infrastructure.Consumers;

public class PaymentsEventsConsumer : BackgroundService
{
    private readonly ILogger<PaymentsEventsConsumer> _logger;
    private readonly Lazy<IConsumer<Ignore, string>> _consumer;
    private readonly string _bootstrapServers;
    private readonly IServiceScopeFactory _scopeFactory;

    private const string EventTypeHeader = "event-type";
    private const string PaymentsTopicName = "payments-events";

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
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        if (!CheckBrokersAndLog(_bootstrapServers))
            return;
        _consumer.Value.Subscribe(PaymentsTopicName);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Value.Consume(stoppingToken);
                var eventTypeHeader = result.Message.Headers.FirstOrDefault(h => h.Key == EventTypeHeader);
                if (eventTypeHeader is null)
                    throw new InvalidOperationException($"Missing header: {EventTypeHeader}");
                var stringEventType = eventTypeHeader.GetValueBytes() is { } bytes
                    ? System.Text.Encoding.UTF8.GetString(bytes)
                    : null;
                _logger.LogInformation(
                    $"Got event \"{eventTypeHeader}\" for {nameof(Booking)} with {nameof(BookRef)} " +
                    $"\"{result.Message.Value}\".");
                var eventType = GetEventTypeOrThrow(stringEventType);
                using var scope = _scopeFactory.CreateScope();
                var commandSender = scope.ServiceProvider.GetRequiredService<ICommandSender>();
                await SendCommandDependOnEventType(eventType, commandSender, result.Message.Value, stoppingToken);
            }
            catch (FormatException e)
            {
                _logger.LogError("{ErrorMessage}", e.Message);
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
        }
    }

    private static PaymentEventType GetEventTypeOrThrow(string? stringEventType)
    {
        if (!Enum.TryParse<PaymentEventType>(stringEventType, out var eventType))
        {
            var supportedEventTypes = Enum.GetNames<PaymentEventType>();
            throw new ArgumentException(
                $"Unknown event type. " +
                $"Supported event types: {string.Join(", ", supportedEventTypes)}.");
        }

        return eventType;
    }

    private async Task SendCommandDependOnEventType(
        PaymentEventType eventType,
        ICommandSender commandSender,
        string jsonPayload,
        CancellationToken cancellationToken)
    {
        var command = eventType switch
        {
            PaymentEventType.PaymentConfirmed =>
                (ICommand)new MarkBookingAsPaidCommand(BookRef.FromString(
                    JsonSerializer.Deserialize<PaymentConfirmedIntegrationEvent>(jsonPayload)!.BookRef)),
            PaymentEventType.PaymentCancelled =>
                new CancelBookingCommand(BookRef.FromString(
                    JsonSerializer.Deserialize<PaymentCancelledIntegrationEvent>(jsonPayload)!.BookRef)),
            _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
        };

        await commandSender.SendAsync(command, cancellationToken);
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
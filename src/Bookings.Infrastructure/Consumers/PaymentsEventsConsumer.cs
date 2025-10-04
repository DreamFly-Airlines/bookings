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
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    
    private const string EventTypeHeader = "event-type";
    private const string PaymentsTopicName = "payments-events";

    public PaymentsEventsConsumer(
        ILogger<PaymentsEventsConsumer> logger,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        var consumerConfig = new ConsumerConfig();
        configuration.GetSection("Kafka:PaymentsEvents:ConsumerSettings").Bind(consumerConfig);
        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        _consumer.Subscribe(PaymentsTopicName);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(stoppingToken);
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
}
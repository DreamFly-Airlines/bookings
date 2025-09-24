using System.Text.Json;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Commands;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Domain.Payments.Abstractions;
using Bookings.Domain.Payments.Enums;
using Bookings.Domain.Payments.Events;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bookings.Infrastructure.Consumers;

public class PaymentsEventsConsumer(
    ILogger<PaymentsEventsConsumer> logger,
    IConsumer<Ignore, string> consumer, 
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private const string EventTypeHeader = "event-type";
    private const string PaymentsTopicName = "payments-events";
    

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe(PaymentsTopicName);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);
                var eventTypeHeader = result.Message.Headers.FirstOrDefault(h => h.Key == EventTypeHeader);
                if (eventTypeHeader is null)
                    throw new InvalidOperationException($"Missing header: {EventTypeHeader}");
                var stringEventType = eventTypeHeader.GetValueBytes() is { } bytes
                    ? System.Text.Encoding.UTF8.GetString(bytes)
                    : null;
                logger.LogInformation(
                    $"Got event \"{eventTypeHeader}\" for {nameof(Booking)} with {nameof(BookRef)} " +
                    $"\"{result.Message.Value}\".");
                var eventType = GetEventTypeOrThrow(stringEventType);
                using var scope = scopeFactory.CreateScope();
                var commandSender = scope.ServiceProvider.GetRequiredService<ICommandSender>();
                await SendCommandDependOnEventType(eventType, commandSender, result.Message.Value, stoppingToken);
            }
            catch (FormatException e)
            {
                logger.LogError(e.Message);
            }
            catch (ConsumeException e)
            {
                logger.LogCritical(e.Message);
            }
            catch (InvalidOperationException e)
            {
                logger.LogCritical(e.Message);
            }
            catch (ArgumentException e)
            {
                logger.LogCritical(e.Message);
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
                (ICommand)new MarkBookingAsPaidCommand(
                    ParseBookRefFromPayloadOrThrow<PaymentConfirmed>(jsonPayload)),

            PaymentEventType.PaymentCancelled =>
                new CancelBookingCommand(
                    ParseBookRefFromPayloadOrThrow<PaymentCancelled>(jsonPayload)),

            _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
        };

        await commandSender.SendAsync(command, cancellationToken);
    }

    private static BookRef ParseBookRefFromPayloadOrThrow<TEventType>(
        string jsonPayload) where TEventType : IPaymentEvent
    {
        var stringBookRef = JsonSerializer.Deserialize<TEventType>(jsonPayload)?.BookRef 
                            ?? throw new ArgumentException(
                                $"Cannot find {nameof(BookRef)} in {jsonPayload}.");
        return BookRef.FromString(stringBookRef);
    }
}
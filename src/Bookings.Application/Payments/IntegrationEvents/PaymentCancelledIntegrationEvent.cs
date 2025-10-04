using Shared.Abstractions.IntegrationEvents;

namespace Bookings.Application.Payments.IntegrationEvents;

public record PaymentCancelledIntegrationEvent(string BookRef) : IIntegrationEvent;
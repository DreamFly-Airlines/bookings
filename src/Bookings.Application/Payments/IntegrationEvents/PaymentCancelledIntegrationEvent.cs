using Bookings.Application.Abstractions;

namespace Bookings.Application.Payments.IntegrationEvents;

public record PaymentCancelledIntegrationEvent(string BookRef) : IIntegrationEvent;
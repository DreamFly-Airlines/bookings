using Bookings.Application.Abstractions;

namespace Bookings.Application.Payments.IntegrationEvents;

public record PaymentConfirmedIntegrationEvent(string BookRef) : IIntegrationEvent;
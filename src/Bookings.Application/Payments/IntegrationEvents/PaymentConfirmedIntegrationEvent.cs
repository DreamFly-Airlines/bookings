using Bookings.Application.Abstractions;
using Shared.Abstractions.IntegrationEvents;

namespace Bookings.Application.Payments.IntegrationEvents;

public record PaymentConfirmedIntegrationEvent(string BookRef) : IIntegrationEvent;
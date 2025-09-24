using Bookings.Domain.Payments.Abstractions;

namespace Bookings.Domain.Payments.Events;

public record PaymentConfirmed(string BookRef) : IPaymentEvent;
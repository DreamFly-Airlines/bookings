using Bookings.Domain.Payments.Abstractions;

namespace Bookings.Domain.Payments.Events;

public record PaymentCancelled(string BookRef) : IPaymentEvent;
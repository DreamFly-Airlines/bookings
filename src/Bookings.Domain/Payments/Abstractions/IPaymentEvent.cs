namespace Bookings.Domain.Payments.Abstractions;

public interface IPaymentEvent
{
    public string BookRef { get; }
}
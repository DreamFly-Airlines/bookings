namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct ContactData
{
    public Email? Email { get; }
    public PhoneNumber? PhoneNumber { get; }

    public ContactData(Email? email = null, PhoneNumber? phoneNumber = null)
    {
        if (email is null && phoneNumber is null)
            throw new FormatException($"At least {nameof(phoneNumber)} or {nameof(email)} is required.");
        Email = email;
        PhoneNumber = phoneNumber;
    }
}
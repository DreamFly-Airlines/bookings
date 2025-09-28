using Bookings.Domain.Bookings.Exceptions;

namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct ContactData
{
    public Email? Email { get; }
    public PhoneNumber? PhoneNumber { get; }

    public ContactData(Email? email = null, PhoneNumber? phoneNumber = null)
    {
        if (email is null && phoneNumber is null)
            throw new InvalidDataFormatException("At least phone number or email is required.");
        Email = email;
        PhoneNumber = phoneNumber;
    }
}
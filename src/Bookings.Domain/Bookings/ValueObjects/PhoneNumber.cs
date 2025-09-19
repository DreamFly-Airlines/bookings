using Bookings.Domain.Shared.Abstractions;

namespace Bookings.Domain.Bookings.ValueObjects;

public readonly record struct PhoneNumber : IStringBackedData<PhoneNumber>
{
    private const int PhoneNumberLength = 10;
    private readonly string _value;

    private PhoneNumber(string value) => _value = value;

    public static PhoneNumber FromString(string phoneNumber)
    {
        var startsWithPlusSeven = phoneNumber.StartsWith("+7");
        var startsWithEight = phoneNumber.StartsWith('8');
        if (!startsWithPlusSeven && !startsWithEight)
            throw new FormatException($"{nameof(phoneNumber)} must start with \"+7\" or \"8\".)");
        var toSkip = startsWithEight ? 1 : 2;
        var numberLength = phoneNumber.Length - toSkip;
        if (numberLength != PhoneNumberLength)
            throw new FormatException(
                $"The part after \"+7\" or \"8\" in {nameof(phoneNumber)} " +
                $"must consist only of {PhoneNumberLength} numbers.");
        for (var i = toSkip; i < phoneNumber.Length; i++)
            if (!char.IsDigit(phoneNumber[i]))
                throw new FormatException(
                    $"The part after \"+7\" or \"8\" in {nameof(phoneNumber)} " +
                    $"must consist only of numbers. Unexpected character \"{phoneNumber[i]}\" at position {i}.");
        return new(phoneNumber);
    }

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber._value;
    
    public override string ToString() => _value;

}
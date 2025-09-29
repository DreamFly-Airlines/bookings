using System.Text.RegularExpressions;
using Bookings.Domain.Bookings.Abstractions;
using Bookings.Domain.Bookings.Exceptions;

namespace Bookings.Domain.Bookings.ValueObjects;

public readonly partial struct Email : IStringBackedData<Email>
{
    private static readonly Regex EmailRegex =
        GetEmailRegex();

    private readonly string _value;

    private Email(string value) => _value = value;

    public static Email FromString(string email)
    {
        if (!EmailRegex.IsMatch(email))
            throw new InvalidDataFormatException("Incorrect format of email");
        return new(email);
    }

    public static implicit operator string(Email email) => email._value;
    
    public override string ToString() => _value;
    
    [GeneratedRegex("""
                    ^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@
                    (?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$
                    """, RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex GetEmailRegex();
}
namespace Bookings.Domain.Bookings.Helpers;

public static class IataCodeChecker
{
    public static void CheckOrThrow(string code, int length)
    {
        if (code.Length != length)
            throw new FormatException($"{nameof(code)} must have {length} digits.");
        for (var i = 0; i < length; i++)
            if (!char.IsDigit(code[i]) && (code[i] > 'Z' || code[i] < 'A'))
                throw new FormatException($"{nameof(code)} must consist only of digits and letters A-Z. " +
                                          $"Unexpected character: {code[i]}.");
    }
}
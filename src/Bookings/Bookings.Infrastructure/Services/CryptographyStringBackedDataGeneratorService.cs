using System.Security.Cryptography;
using System.Text;
using Bookings.Application.Bookings.Services;

namespace Bookings.Infrastructure.Services;

public class CryptographyStringBackedDataGeneratorService : IStringBackedDataGeneratorService
{
    private const string Digits = "0123456789";
    private const string UpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    public string Generate(int length, bool isNumbersAllowed, bool isCapitalLettersAllowed)
    {
        if (length <= 0)
            throw new ArgumentException("Length must be greater than 0.", nameof(length));
        if (!isNumbersAllowed && !isCapitalLettersAllowed)
            throw new ArgumentException("At least numbers or capital letters should be allowed.");
        
        var allowedChars = new StringBuilder();
        if (isNumbersAllowed)
            allowedChars.Append(Digits);
        if (isCapitalLettersAllowed)
            allowedChars.Append(UpperLetters);

        var result = new char[length];
        var buffer = new byte[4];

        using var rng = RandomNumberGenerator.Create();
        for (var i = 0; i < length; i++)
        {
            rng.GetBytes(buffer);
            var randomNumber = BitConverter.ToInt32(buffer, 0) & int.MaxValue;
            result[i] = allowedChars[randomNumber % allowedChars.Length];
        }

        return new string(result);
    }
}
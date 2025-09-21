using Bookings.Application.Bookings.Services;

namespace Bookings.Api.IntegrationTests.Mocks;

public class MockConstStringBackedDataGeneratorService : IStringBackedDataGeneratorService
{
    private const char NumberToRepeat = '1';
    private const char CapitalLetterToRepeat = 'A';

    public string Generate(int length, bool isNumbersAllowed, bool isCapitalLettersAllowed)
    {
        if (isNumbersAllowed)
            return new(NumberToRepeat, length);
        if (isCapitalLettersAllowed)
            return new(CapitalLetterToRepeat, length);
        throw new InvalidOperationException("At least numbers or capital letters must be allowed.");
    }
}
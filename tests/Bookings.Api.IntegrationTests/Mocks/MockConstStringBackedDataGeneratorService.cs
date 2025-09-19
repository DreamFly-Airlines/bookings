using Bookings.Application.Bookings.Services;

namespace Bookings.Api.IntegrationTests.Mocks;

public class MockConstStringBackedDataGeneratorService : IStringBackedDataGeneratorService
{
    private const char ValueToRepeat = '1';
    
    public string Generate(int length, bool isNumbersAllowed, bool isCapitalLettersAllowed)
        => new(Enumerable.Repeat(ValueToRepeat, length).ToArray());
}
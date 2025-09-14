namespace Bookings.Application.Services;

public interface IStringBackedDataGeneratorService
{
    public string Generate(int length, bool isNumbersAllowed, bool isCapitalLettersAllowed);
}
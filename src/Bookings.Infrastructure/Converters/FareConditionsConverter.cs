using Bookings.Domain.Bookings.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class FareConditionsConverter() : ValueConverter<FareConditions, string>(
    enumConditions => ConvertToString(enumConditions), 
    stringConditions => ConvertToFareConditions(stringConditions))
{
    private const string EconomyName = "Economy";
    private const string ComfortName = "Comfort";
    private const string BusinessName = "Business";
    
    private static string ConvertToString(FareConditions fareConditions) => fareConditions switch
    {
        FareConditions.Economy => EconomyName,
        FareConditions.Comfort => ComfortName,
        FareConditions.Business => BusinessName,
        _ => throw new ArgumentOutOfRangeException(nameof(fareConditions), fareConditions, null)
    };
    
    private static FareConditions ConvertToFareConditions(string fareConditions) => fareConditions switch
    {
        EconomyName => FareConditions.Economy,
        ComfortName => FareConditions.Comfort,
        BusinessName => FareConditions.Business,
        _ => throw new ArgumentOutOfRangeException(nameof(fareConditions), fareConditions, null)
    };
}
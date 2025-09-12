using Bookings.Domain.Bookings.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookings.Infrastructure.Converters;

public class WeekDayArrayConverter() : ValueConverter<List<WeekDay>, string[]>(
    enumDays => enumDays.Select(ConvertToString).ToArray(), 
    stringDays => stringDays.Select(ConvertToWeekDay).ToList())
{
    private const string MondayName = "0";
    private const string TuesdayName = "1";
    private const string WednesdayName = "2";
    private const string ThursdayName = "3";
    private const string FridayName = "4";
    private const string SaturdayName = "5";
    private const string SundayName = "6";
    
    private static string ConvertToString(WeekDay weekDay) => weekDay switch
    {
        WeekDay.Monday => MondayName,
        WeekDay.Tuesday => TuesdayName,
        WeekDay.Wednesday => WednesdayName,
        WeekDay.Thursday => ThursdayName,
        WeekDay.Friday => FridayName,
        WeekDay.Saturday => SaturdayName,
        WeekDay.Sunday => SundayName,
        _ => throw new ArgumentOutOfRangeException(nameof(weekDay), weekDay, null)
    };
    
    private static WeekDay ConvertToWeekDay(string weekDay) => weekDay switch
    {
        MondayName => WeekDay.Monday,
        TuesdayName => WeekDay.Tuesday,
        WednesdayName => WeekDay.Wednesday,
        ThursdayName => WeekDay.Thursday,
        FridayName => WeekDay.Friday,
        SaturdayName => WeekDay.Saturday,
        SundayName => WeekDay.Sunday,
        _ => throw new ArgumentOutOfRangeException(nameof(weekDay), weekDay, null)
    };
}
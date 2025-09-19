using System.ComponentModel.DataAnnotations;

namespace Bookings.Api.Dto;

public record FlightsSearchDto
{
    [Required] public string DepartureCity { get; init; } = null!;
    [Required] public string ArrivalCity { get; init; } = null!;
    [Required] public DateOnly DepartureDate { get; init; }
    [Required] public int PassengersCount { get; init; }
}
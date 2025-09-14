using System.ComponentModel.DataAnnotations;
using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Api.Dto;

public record BookingRequestDto
{
    // TODO: use record instead of tuple
    [Required]
    [MinLength(1, ErrorMessage = "At least 1 User ID is required.")]
    public HashSet<(string PassengerId, string PassengerName, ContactData ContactData)>
        PassengersInfos { get; init; } = null!;
    
    [Required]
    public FareConditions FareConditions { get; init; }
}
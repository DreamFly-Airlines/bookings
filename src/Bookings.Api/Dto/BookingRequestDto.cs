using System.ComponentModel.DataAnnotations;
using Bookings.Domain.Bookings.Entities;
using Bookings.Domain.Bookings.Enums;

namespace Bookings.Api.Dto;

public record BookingRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "At least 1 passenger is required.")]
    public HashSet<PassengerInfoDto> PassengersInfos { get; init; } = null!;
    
    [Required]
    [MinLength(1, ErrorMessage = $"At least one {nameof(Flight)} for itinerary is required.")]
    public IEnumerable<int> ItineraryFlightsIds { get; init; } = null!;
    
    [Required]
    public FareConditions FareConditions { get; init; }
}
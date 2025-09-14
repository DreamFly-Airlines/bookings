using System.ComponentModel.DataAnnotations;

namespace Bookings.Api.Dto;

public record BookingRequestDto
{
    [Required] 
    [MinLength(1, ErrorMessage = "At least 1 User ID is required.")]
    public HashSet<string> UserDocumentsIds { get; init; }
}
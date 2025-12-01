using Bookings.Api.Authorization;
using Bookings.Application.Bookings.Queries;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Queries;

namespace Bookings.Api.Controllers;

[ApiController]
[Route("/api/bookings")]
public class BookingsController(IQuerySender querySender) : Controller
{
    [HttpGet("{bookRef}")]
    [Authorize(Policy = Policies.HasNameIdentifier)]
    public async Task<IActionResult> GetBooking([FromRoute] string bookRef)
    {
        var parsedBookRef = BookRef.FromString(bookRef);
        var query = new GetBookingQuery(parsedBookRef);
        var booking = await querySender.SendAsync(query);
        return Ok(booking);
    }
}
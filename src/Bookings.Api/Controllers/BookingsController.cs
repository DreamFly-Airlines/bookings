using Bookings.Application.Bookings.Queries;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Queries;

namespace Bookings.Api.Controllers;

[ApiController]
[Route("/api/bookings")]
public class BookingsController(IQuerySender querySender) : Controller
{
    [HttpGet("{bookRef}")]
    public async Task<IActionResult> GetBooking([FromQuery] string bookRef)
    {
        var parsedBookRef = BookRef.FromString(bookRef);
        var query = new GetBookingQuery(parsedBookRef);
        var booking = await querySender.SendAsync(query);
        return Ok(booking);
    }
}
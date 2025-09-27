using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Queries;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Api.Controllers;

[ApiController]
[Route("/api/bookings")]
public class BookingsController(IQuerySender querySender) : Controller
{
    [HttpGet("{bookRef}")]
    public async Task<IActionResult> GetBooking([FromQuery] string bookRef)
    {
        try
        {
            var parsedBookRef = BookRef.FromString(bookRef);
            var query = new GetBookingQuery(parsedBookRef);
            var booking = await querySender.SendAsync(query);
            if (booking is null)
                return NotFound();
            return Ok(booking);
        }
        catch (FormatException e)
        {
            return BadRequest(e.Message);
        }
    }
    
}
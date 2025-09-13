using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Api.Controllers;

public class BookingController(IQuerySender sender) : Controller
{
    [HttpGet("/flights")]
    public async Task<IActionResult> FindMatchingFlights(
        [FromBody] string departureCity, 
        [FromBody] string arrivalCity, 
        [FromBody] DateTime departureDate, 
        [FromQuery] int passengersCount)
    {
        var query = new GetMatchingFlightsQuery(departureCity, arrivalCity, departureDate, passengersCount);
        var flights = await sender.SendAsync(query);
        return Ok(flights);
    }
}
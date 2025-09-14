using Bookings.Api.Dto;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Commands;
using Bookings.Application.Bookings.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController(
    ICommandSender commandSender,
    IQuerySender querySender) : Controller
{
    [HttpGet("search")]
    public async Task<IActionResult> FindMatchingFlights([FromQuery] FlightsSearchDto dto)
    {
        var query = new SearchMatchingFlightsQuery(
            dto.DepartureCity, 
            dto.ArrivalCity, 
            dto.DepartureDate, 
            dto.PassengersCount);
        var flights = await querySender.SendAsync(query);
        return Ok(flights);
    }

    [HttpPost("book/{flightId}")]
    public async Task<IActionResult> BookFlight(string flightId, [FromBody] BookingRequestDto request)
    {
        var bookCommand = new BookFlightCommand(flightId, request.UserDocumentsIds);
        await commandSender.SendAsync(bookCommand);
        return Ok();
    }
}
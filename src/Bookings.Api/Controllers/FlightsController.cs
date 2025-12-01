using System.Security.Claims;
using Bookings.Api.Authorization;
using Bookings.Api.Dto;
using Bookings.Application.Bookings.Commands;
using Bookings.Application.Bookings.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Commands;
using Shared.Abstractions.Queries;

namespace Bookings.Api.Controllers;

[ApiController]
[Route("api/flights")]
public class FlightsController(
    ICommandSender commandSender,
    IQuerySender querySender) : Controller
{
    [HttpGet]
    public async Task<IActionResult> FindMatchingFlights([FromQuery] SearchFlightsRequest dto)
    {
        var query = new SearchFlightsItineraryQuery(
            dto.DepartureCity, 
            dto.ArrivalCity, 
            dto.DepartureDate, 
            dto.PassengersCount);
        var flights = await querySender.SendAsync(query);
        return Ok(flights);
    }

    [HttpPost("book")]
    [Authorize(Policy = Policies.HasNameIdentifier)]
    public async Task<IActionResult> MakeBooking([FromBody] MakeBookingRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var bookCommand = new MakeBookingCommand(
            userId,
            request.ItineraryFlightsIds, 
            request.PassengersInfos, 
            request.FareConditions);
        var bookRef = await commandSender.SendAsync(bookCommand);
        return CreatedAtAction(
            actionName: nameof(BookingsController.GetBooking),
            controllerName: "Bookings",
            new { bookRef },
            bookRef);
    }
}
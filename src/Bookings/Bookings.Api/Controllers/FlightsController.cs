using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Bookings.Api.Dto;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController(IQuerySender sender) : Controller
{
    [HttpGet]
    public async Task<IActionResult> FindMatchingFlights([FromQuery] FlightsSearchDto dto)
    {
        var departureUtcDate = DateTime.SpecifyKind(dto.DepartureDate, DateTimeKind.Utc);
        var query = new GetMatchingFlightsQuery(
            dto.DepartureCity, 
            dto.ArrivalCity, 
            departureUtcDate, 
            dto.PassengersCount);
        var flights = await sender.SendAsync(query);
        return Ok(flights);
    }
}
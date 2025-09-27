using Bookings.Api.Dto;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Commands;
using Bookings.Application.Bookings.Queries;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Api.Controllers;

[ApiController]
[Route("api/flights")]
public class FlightsController(
    ICommandSender commandSender,
    IQuerySender querySender) : Controller
{
    [HttpGet]
    public async Task<IActionResult> FindMatchingFlights([FromQuery] FlightsSearchDto dto)
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
    public async Task<IActionResult> MakeBooking([FromBody] BookingRequestDto request)
    {
        try
        {
            var passengersInfos = request.PassengersInfos.Select(dto =>
            {
                var email = dto.ContactDataDto.Email is null 
                    ? (Email?)null 
                    : Email.FromString(dto.ContactDataDto.Email);
                var phoneNumber = dto.ContactDataDto.PhoneNumber is null 
                    ? (PhoneNumber?)null 
                    : PhoneNumber.FromString(dto.ContactDataDto.PhoneNumber);
                return (dto.PassengerId, dto.PassengerName, new ContactData(email: email, phoneNumber: phoneNumber));
            }).ToHashSet();
            var bookCommand = new MakeBookingCommand(
                request.ItineraryFlightsIds, passengersInfos, request.FareConditions);
            await commandSender.SendAsync(bookCommand);
            return Created();
        }
        catch (FormatException ex)
        {
            ModelState.AddModelError($"incorrectFormat", ex.Message);
            return BadRequest(ModelState);
        }
    }
}
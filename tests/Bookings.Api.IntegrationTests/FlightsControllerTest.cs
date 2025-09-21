using System.Net;
using System.Net.Http.Json;
using Bookings.Api.Dto;
using Bookings.Api.IntegrationTests.Abstractions;
using Bookings.Api.IntegrationTests.Factories;
using Bookings.Api.IntegrationTests.Mocks;
using Bookings.Application.Bookings.Dto;
using Bookings.Domain.Bookings.Enums;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Api.IntegrationTests;

public class FlightsControllerTest(BookingsAppFactory factory) : BaseDatabaseIntegrationTest(factory)
{
    [Fact]
    public void TestHost_Builds_Successfully()
    {
        Assert.NotNull(Client);
        Assert.NotNull(DbContext);
    }

    [Fact]
    public async Task MakeBooking_FromMoscowToBryansk_CreatesBookingInDatabase()
    {
        const int moscowBryanskFlightId = 19142;
        var moscowBryanskFlight = await DbContext.Flights
            .FirstOrDefaultAsync(f => f.FlightId == moscowBryanskFlightId);
        Assert.NotNull(moscowBryanskFlight);
        
        var passengerInfo = new PassengerInfoDto("9999 999999", "TEST PASSENGER",
            new ContactData(email: Email.FromString("test@test.com")));
        var request = new BookingRequestDto
        {
            ItineraryFlightsIds = new[] { moscowBryanskFlightId },
            PassengersInfos = new HashSet<PassengerInfoDto> { passengerInfo },
            FareConditions = FareConditions.Economy
        };
        
        var response = await Client.PostAsJsonAsync("/api/flights/book", request, SerializerOptions);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var generator = new MockConstStringBackedDataGeneratorService();
        var expectedBookRef = BookRef.FromString(
            generator.Generate(BookRef.BookRefLength, true, true));
        var expectedTicketNo = TicketNo.FromString(generator.Generate(
            TicketNo.TicketNoLength, true, true));

        var bookingInDb = await DbContext.Bookings
            .Include(b => b.Tickets)
            .ThenInclude(t => t.TicketFlights)
            .FirstOrDefaultAsync(b => b.BookRef == expectedBookRef);

        Assert.NotNull(bookingInDb);
        Assert.Equal(1000m, bookingInDb.TotalAmount);

        var ticketInDb = Assert.Single(bookingInDb.Tickets);

        Assert.Equal(expectedTicketNo, ticketInDb.TicketNo);
        Assert.Equal(passengerInfo.PassengerId, ticketInDb.PassengerId);
        Assert.Equal(passengerInfo.PassengerName, ticketInDb.PassengerName);
        Assert.Equal(passengerInfo.ContactData, ticketInDb.ContactData);

        var ticketFlightInDb = Assert.Single(ticketInDb.TicketFlights);
        Assert.Equal(moscowBryanskFlightId, ticketFlightInDb.FlightId);
    }
}
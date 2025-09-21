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
        const string passengerId = "9999 999999";
        const string passengerName = "TEST PASSENGER";
        const string email = "test@test.com";
        const FareConditions fareConditions = FareConditions.Economy;
        
        var moscowBryanskFlight = await DbContext.Flights
            .FirstOrDefaultAsync(f => f.FlightId == moscowBryanskFlightId);
        Assert.NotNull(moscowBryanskFlight);
        
        var passengerInfo = new PassengerInfoDto(passengerId, passengerName,
            new ContactData(email: Email.FromString(email)));
        var request = new BookingRequestDto
        {
            ItineraryFlightsIds = new[] { moscowBryanskFlightId },
            PassengersInfos = new HashSet<PassengerInfoDto> { passengerInfo },
            FareConditions = fareConditions
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
        Assert.Equal(MockConstItineraryPricingService.MockPrice, bookingInDb.TotalAmount);
        Assert.Single(bookingInDb.Tickets);

        var ticketInDb = await DbContext.Tickets.FirstOrDefaultAsync(t => t.BookRef == bookingInDb.BookRef);
        
        Assert.NotNull(ticketInDb);
        Assert.Single(ticketInDb.TicketFlights);
        Assert.Equal(expectedTicketNo, ticketInDb.TicketNo);
        Assert.Equal(passengerInfo.PassengerId, ticketInDb.PassengerId);
        Assert.Equal(passengerInfo.PassengerName, ticketInDb.PassengerName);
        Assert.Equal(passengerInfo.ContactData, ticketInDb.ContactData);

        var ticketFlightInDb = await DbContext.TicketFlights
            .FirstOrDefaultAsync(tf => tf.TicketNo == ticketInDb.TicketNo);
        Assert.NotNull(ticketFlightInDb);
        Assert.Equal(moscowBryanskFlightId, ticketFlightInDb.FlightId);
        Assert.Equal(fareConditions, ticketFlightInDb.FareConditions);
        Assert.Equal(MockConstItineraryPricingService.MockPrice, ticketFlightInDb.Amount);
    }
}
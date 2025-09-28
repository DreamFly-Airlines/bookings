using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Entities;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Bookings.Application.Bookings.Commands;

public class MakeBookingCommandHandler(
    ILogger<MakeBookingCommandHandler> logger,
    IItineraryPricingService pricingService,
    IClockService clockService,
    IBookingRepository bookingRepository,
    IStringBackedDataGeneratorService generator) : ICommandHandler<MakeBookingCommand>
{
    public async Task HandleAsync(MakeBookingCommand command, CancellationToken cancellationToken = default)
    {
        var bookRef = BookRef.FromString(
            generator.Generate(BookRef.BookRefLength, true, true));
        var ticketCost = await pricingService.CalculatePriceAsync(
            command.ItineraryFlightsIds, command.FareConditions, cancellationToken);
        var ticketsInfo = command.PassengersInfos
            .Select(passengerInfo =>
        {
            var ticketNo = TicketNo.FromString(
                generator.Generate(TicketNo.TicketNoLength, true, false));
            var email = passengerInfo.ContactDataDto.Email is null 
                ? (Email?)null 
                : Email.FromString(passengerInfo.ContactDataDto.Email);
            var phoneNumber = passengerInfo.ContactDataDto.PhoneNumber is null 
                ? (PhoneNumber?)null 
                : PhoneNumber.FromString(passengerInfo.ContactDataDto.PhoneNumber);
            var contactData = new ContactData(email: email, phoneNumber: phoneNumber);
            return (
                TicketNo: ticketNo, 
                TicketCost: ticketCost, 
                Passenger: new Passenger(
                    passengerInfo.PassengerId, passengerInfo.PassengerName, contactData));
        });
        var bookDate = await clockService.NowAsync(cancellationToken);
        var booking = new Booking(
            bookRef, 
            bookDate, 
            command.FareConditions,
            command.ItineraryFlightsIds, 
            ticketsInfo);
        await bookingRepository.AddAsync(booking, cancellationToken);
        logger.LogInformation(
            "{BookingName} with {BookRefName} \"{BookingBookRef}\" created.", 
            nameof(Booking), nameof(BookRef), booking.BookRef);
    }
}
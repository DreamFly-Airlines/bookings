using System.ComponentModel.DataAnnotations;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Exceptions;
using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Entities;
using Bookings.Domain.Bookings.Exceptions;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;
using Microsoft.Extensions.Logging;
using ValidationException = Bookings.Application.Bookings.Exceptions.ValidationException;

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
                try
                {
                    var ticketNo = TicketNo.FromString(
                        generator.Generate(TicketNo.TicketNoLength, true, false));
                    var email = string.IsNullOrEmpty(passengerInfo.ContactDataDto.Email)
                        ? (Email?)null
                        : Email.FromString(passengerInfo.ContactDataDto.Email);
                    var phoneNumber = string.IsNullOrEmpty(passengerInfo.ContactDataDto.PhoneNumber)
                        ? (PhoneNumber?)null
                        : PhoneNumber.FromString(passengerInfo.ContactDataDto.PhoneNumber);
                    var contactData = new ContactData(email: email, phoneNumber: phoneNumber);
                    return (
                        TicketNo: ticketNo,
                        TicketCost: ticketCost,
                        Passenger: new Passenger(
                            passengerInfo.PassengerId, passengerInfo.PassengerName, contactData));
                }
                catch (InvalidDataFormatException ex)
                {
                    throw GetInvalidContactDataForPassengerException(
                        ex.Message, passengerInfo.ContactDataDto.Email, passengerInfo.ContactDataDto.PhoneNumber);
                }
            });
        var bookDate = await clockService.NowAsync(cancellationToken);
        var booking = new Booking(bookRef, bookDate, command.FareConditions, command.ItineraryFlightsIds, ticketsInfo);
        await bookingRepository.AddAsync(booking, cancellationToken);
        logger.LogInformation(
            "{BookingName} with {BookRefName} \"{BookingBookRef}\" created.",
            nameof(Booking), nameof(BookRef), booking.BookRef);
    }

    private ValidationException GetInvalidContactDataForPassengerException(string message, string? email, string? phoneNumber)
    {
        var state = new EntityStateInfo(nameof(ContactData),
            (nameof(Email), email ?? string.Empty),
            (nameof(PhoneNumber), phoneNumber ?? string.Empty));
        return new ValidationException(message, true, state);
    }
}
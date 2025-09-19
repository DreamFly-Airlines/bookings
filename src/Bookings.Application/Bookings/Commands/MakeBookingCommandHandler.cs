using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Entities;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Commands;

public class MakeBookingCommandHandler(
    IItineraryPricingService pricingService,
    IClockService clockService,
    IBookingRepository bookingRepository,
    IStringBackedDataGeneratorService generator) : ICommandHandler<MakeBookingCommand>
{
    public async Task HandleAsync(MakeBookingCommand command, CancellationToken cancellationToken = default)
    {
        var bookRef = BookRef.FromString(generator.Generate(BookRef.BookRefLength, true, true));
        var ticketCost = await pricingService.CalculatePriceAsync(
            command.ItineraryFlightsIds, command.FareConditions, cancellationToken);
        var ticketsInfo = command.PassengersInfos
            .Select(passengerInfo =>
        {
            var ticketNo = TicketNo.FromString(
                generator.Generate(TicketNo.TicketNoLength, true, false));
            return (
                TicketNo: ticketNo, 
                TicketCost: ticketCost, 
                Passenger: new Passenger(
                    passengerInfo.PassengerId, passengerInfo.PassengerName, passengerInfo.ContactData));
        });
        var bookDate = await clockService.NowAsync(cancellationToken);
        var booking = new Booking(
            bookRef, 
            bookDate, 
            command.FareConditions, 
            command.ItineraryFlightsIds, 
            ticketsInfo);
        await bookingRepository.AddAsync(booking, cancellationToken);
    }
}
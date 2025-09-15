using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.AggregateRoots;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;

namespace Bookings.Application.Bookings.Commands;

public class MakeBookingCommandHandler(
    IClockService clockService,
    IBookingRepository bookingRepository,
    IStringBackedDataGeneratorService generator) : ICommandHandler<MakeBookingCommand>
{
    public async Task HandleAsync(MakeBookingCommand command, CancellationToken cancellationToken = default)
    {
        var bookRef = BookRef.FromString(
            generator.Generate(BookRef.BookRefLength, true, true));
        const decimal ticketCost = 1000; // TODO: calculate tickets costs
        var ticketsInfo = command.PassengersInfos.Select(passengerInfo =>
        {
            var ticketNo = TicketNo.FromString(
                generator.Generate(TicketNo.TicketNoLength, true, false));
            return (
                TicketNo: ticketNo, 
                TicketCost: ticketCost, 
                passengerInfo.PassengerId, 
                passengerInfo.PassengerName, 
                passengerInfo.ContactData);
        });
        var bookDate = await clockService.NowAsync(cancellationToken);
        var booking = new Booking(
            bookRef, 
            bookDate, 
            command.FareConditions, 
            command.FlightsIds, 
            ticketsInfo);
        await bookingRepository.AddAsync(booking, cancellationToken);
    }
}
using Bookings.Application.Bookings.ReadModels;
using Shared.Abstractions.Queries;

namespace Bookings.Application.Bookings.Queries;

public record GetBookingsQuery(string UserId) : IQuery<List<BookingReadModel>>;
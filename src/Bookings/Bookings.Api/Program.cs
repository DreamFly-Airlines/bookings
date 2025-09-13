using Bookings.Api.Extensions;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Queries;
using Bookings.Infrastructure.Persistence;
using Bookings.Infrastructure.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsql<BookingsDbContext>(builder.Configuration.GetConnectionString("BookingsDb"));
builder.Services.AddScoped<IQuerySender, ServiceProviderQuerySender>();
builder.Services.AddQueryHandlers(typeof(GetMatchingFlightsQueryHandler).Assembly);
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.Run();
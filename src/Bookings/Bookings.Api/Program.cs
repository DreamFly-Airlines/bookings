using System.Globalization;
using Bookings.Api.Extensions;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Queries;
using Bookings.Application.Bookings.ReadModels.Repositories;
using Bookings.Infrastructure.Persistence;
using Bookings.Infrastructure.Queries;
using Bookings.Infrastructure.Repositories;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsql<BookingsDbContext>(builder.Configuration.GetConnectionString("BookingsDb"));
builder.Services.AddScoped<IQuerySender, ServiceProviderQuerySender>();
builder.Services.AddScoped<IFlightReadModelRepository, SqlFlightReadModelRepository>();
builder.Services.AddQueryHandlers(typeof(GetMatchingFlightsQueryHandler).Assembly);
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
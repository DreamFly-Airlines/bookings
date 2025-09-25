using Bookings.Api.Extensions;
using Bookings.Application.Abstractions;
using Bookings.Application.Bookings.Commands;
using Bookings.Application.Bookings.EventHandlers;
using Bookings.Application.Bookings.Queries;
using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Domain.Bookings.ValueObjects;
using Bookings.Infrastructure;
using Bookings.Infrastructure.Commands;
using Bookings.Infrastructure.Persistence;
using Bookings.Infrastructure.Queries;
using Bookings.Infrastructure.Repositories;
using Bookings.Infrastructure.Serialization;
using Bookings.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsql<BookingsDbContext>(builder.Configuration.GetConnectionString("BookingsDb"));

builder.Services.AddScoped<IQuerySender, ServiceProviderQuerySender>();
builder.Services.AddScoped<ICommandSender, ServiceProviderCommandSender>();
builder.Services.AddScoped<IFlightSearchingService, SqlFlightSearchingService>();
builder.Services.AddScoped<IBookingRepository, SqlBookingRepository>();
builder.Services.AddScoped<IClockService, SqlClockService>();
builder.Services.AddScoped<IItineraryPricingService, MockItineraryPricingService>();
builder.Services.AddSingleton<IStringBackedDataGeneratorService, CryptographyStringBackedDataGeneratorService>();

builder.Services.AddQueryHandlers(typeof(SearchFlightsItineraryQueryQueryHandler).Assembly);
builder.Services.AddCommandHandlers(typeof(MakeBookingCommandHandler).Assembly);
builder.Services.AddEventHandlers(typeof(BookingCancelledEventHandler).Assembly);
builder.Services.AddKafkaConsumers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new StringBackedDataJsonConverterFactory());
        opts.JsonSerializerOptions.Converters.Add(new ContactDataJsonConverter());
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
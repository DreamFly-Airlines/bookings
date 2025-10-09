using Bookings.Api.ExceptionHandling;
using Bookings.Api.Extensions;
using Bookings.Application.Bookings.Commands;
using Bookings.Application.Bookings.EventHandlers;
using Bookings.Application.Bookings.Queries;
using Bookings.Application.Bookings.Services;
using Bookings.Domain.Bookings.Repositories;
using Bookings.Infrastructure.Persistence;
using Bookings.Infrastructure.Repositories;
using Bookings.Infrastructure.Serialization;
using Bookings.Infrastructure.Services;
using Shared.Abstractions.Commands;
using Shared.Abstractions.Events;
using Shared.Abstractions.IntegrationEvents;
using Shared.Abstractions.Queries;
using Shared.Extensions.ServiceCollection;
using Shared.Infrastructure.Commands;
using Shared.Infrastructure.Events;
using Shared.Infrastructure.IntegrationEvents;
using Shared.Infrastructure.Queries;
using Shared.IntegrationEvents.Payments;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsql<BookingsDbContext>(builder.Configuration.GetConnectionString("BookingsDb"));

builder.Services.AddScoped<IQuerySender, ServiceProviderQuerySender>();
builder.Services.AddScoped<ICommandSender, ServiceProviderCommandSender>();
builder.Services.AddScoped<IEventPublisher, ServiceProviderEventPublisher>();
builder.Services.AddScoped<IIntegrationEventPublisher, ServiceProviderIntegrationEventPublisher>();
builder.Services.AddScoped<IFlightSearchingService, SqlFlightSearchingService>();
// builder.Services.AddScoped<IBookingRepository, SqlBookingRepository>();
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddScoped<IClockService, SqlClockService>();
builder.Services.AddScoped<IItineraryPricingService, MockItineraryPricingService>();
builder.Services.AddSingleton<IStringBackedDataGeneratorService, CryptographyStringBackedDataGeneratorService>();

builder.Services.AddQueryHandlers(typeof(SearchFlightsItineraryQueryQueryHandler).Assembly);
builder.Services.AddCommandHandlers(typeof(MakeBookingCommandHandler).Assembly);
builder.Services.AddDomainEventHandlers(typeof(BookingCancelledEventHandler).Assembly);
builder.Services.AddIntegrationEventHandlers(typeof(PaymentCancelledIntegrationEvent).Assembly);
builder.Services.AddKafkaConsumers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new StringBackedDataJsonConverterFactory());
        opts.JsonSerializerOptions.Converters.Add(new ContactDataJsonConverter());
    });
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
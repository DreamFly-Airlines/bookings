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

builder.Services.AddQueryHandlers(typeof(SearchMatchingFlightsQueryHandler).Assembly);

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
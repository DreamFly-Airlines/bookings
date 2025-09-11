using Bookings.Infrastructure.Persistence;
using Bookings.Infrastructure.Persistence.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsql<BookingsDbContext>(builder.Configuration.GetConnectionString("BookingsDb"));

var app = builder.Build();

app.UseHttpsRedirection();
app.Run();
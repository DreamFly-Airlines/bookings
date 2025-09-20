using System.Data.Common;
using Bookings.Api.IntegrationTests.Extensions;
using Bookings.Api.IntegrationTests.Mocks;
using Bookings.Application.Bookings.Services;
using Bookings.Infrastructure.Persistence;
using Bookings.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Bookings.Api.IntegrationTests.Factories;

public class BookingsAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
            // NOTE: this is a real database that is being used in production
            // consider replacing with test database
            
            services.RemoveServiceDescriptorIfExists<DbContextOptions<BookingsDbContext>>();
            services.RemoveServiceDescriptorIfExists<BookingsDbContext>();
            
            // NOTE: the connection in the API request handler should be the same as in 
            // BaseDatabaseIntegrationTest to ensure it runs within the test's transaction
            // normally, we should use a test database that resets for every test (check Respawn or an analog)

            services.AddSingleton<DbConnection>(_ =>
            {
                var connection = new NpgsqlConnection(
                    "Host=localhost;Port=5432;Database=DreamFly;Username=postgres;Password=postgres");
                connection.Open();
                return connection;
            });
            services.AddDbContext<BookingsDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseNpgsql(connection);
            });
            
            services.ReplaceSingletonService<IStringBackedDataGeneratorService, 
                MockConstStringBackedDataGeneratorService>();
            services.ReplaceScopedService<IItineraryPricingService, 
                MockConstItineraryPricingService>();
        });
        base.ConfigureWebHost(builder);
    }
}
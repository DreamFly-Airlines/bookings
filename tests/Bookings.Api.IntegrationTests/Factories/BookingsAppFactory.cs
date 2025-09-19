using Bookings.Api.IntegrationTests.Extensions;
using Bookings.Api.IntegrationTests.Mocks;
using Bookings.Application.Bookings.Services;
using Bookings.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Api.IntegrationTests.Factories;

internal class BookingsAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // NOTE: this is a real database that is being used in production
            // consider replacing with test database
            
            services.RemoveServiceDescriptorIfExists<BookingsDbContext>();
            services.AddNpgsql<BookingsDbContext>(
                "Host=localhost;Port=5432;Database=DreamFly;Username=postgres;Password=postgres");
            services.ReplaceSingletonService<IStringBackedDataGeneratorService, 
                MockConstStringBackedDataGeneratorService>();
        });
        base.ConfigureWebHost(builder);
    }
}
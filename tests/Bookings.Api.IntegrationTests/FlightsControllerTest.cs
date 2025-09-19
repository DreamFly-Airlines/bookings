using Bookings.Api.IntegrationTests.Abstractions;
using Bookings.Api.IntegrationTests.Factories;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Bookings.Api.IntegrationTests;

public class FlightsControllerTest(BookingsAppFactory factory) : BaseDatabaseIntegrationTest(factory)
{
    [Fact]
    public void TestHost_Builds_Successfully()
    {
        Assert.NotNull(Client);
        Assert.NotNull(DbContext);
    }
}
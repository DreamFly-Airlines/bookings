using Bookings.Api.IntegrationTests.Abstractions;
using Bookings.Api.IntegrationTests.Factories;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Bookings.Api.IntegrationTests;

public class FlightsControllerTest : BaseDatabaseIntegrationTest
{
    internal FlightsControllerTest(BookingsAppFactory factory) : base(factory) { }
}
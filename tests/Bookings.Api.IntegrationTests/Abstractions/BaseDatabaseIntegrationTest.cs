using Bookings.Api.IntegrationTests.Factories;
using Bookings.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Api.IntegrationTests.Abstractions;

public abstract class BaseDatabaseIntegrationTest : IClassFixture<BookingsAppFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private IDbContextTransaction? _transaction;
    
    protected HttpClient Client { get; }
    protected BookingsDbContext DbContext { get; }

    protected BaseDatabaseIntegrationTest(BookingsAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Client = factory.CreateClient();
        DbContext = _scope.ServiceProvider.GetRequiredService<BookingsDbContext>();
    }
    
    public async Task InitializeAsync()
    {
        _transaction = await DbContext.Database.BeginTransactionAsync();
    }

    public async Task DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
        _scope.Dispose();
    }
}
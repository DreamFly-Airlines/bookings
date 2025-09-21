using System.Text.Json;
using Bookings.Api.IntegrationTests.Factories;
using Bookings.Infrastructure.Persistence;
using Bookings.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Api.IntegrationTests.Abstractions;

public abstract class BaseDatabaseIntegrationTest : IClassFixture<BookingsAppFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private IDbContextTransaction? _transaction;
    private readonly BookingsAppFactory _factory;
    protected JsonSerializerOptions SerializerOptions => GetSerializerOptions();
    
    protected HttpClient Client { get; }
    protected BookingsDbContext DbContext { get; }

    protected BaseDatabaseIntegrationTest(BookingsAppFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        Client = factory.CreateClient();
        DbContext = _scope.ServiceProvider.GetRequiredService<BookingsDbContext>();
    }
    
    public async Task InitializeAsync()
    {
        _transaction = await DbContext.Database.BeginTransactionAsync();
        _factory.ActiveTransaction = _transaction.GetDbTransaction();
    }

    public async Task DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _factory.ActiveTransaction = null;
        }
        _scope.Dispose();
    }

    private static JsonSerializerOptions GetSerializerOptions() 
        => new() 
        {
            Converters = { new StringBackedDataJsonConverterFactory() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
}
using System.Data.Common;
using DotNet.Testcontainers.Builders;
using FitSync.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Testcontainers.PostgreSql;

namespace FitSync.IntegrationTests.Configurations;

public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("FitSyncDb")
        .WithUsername("admin")
        .WithPassword("admin")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();
    public FitSyncDbContext FitSyncDbContext { get; private set; } = default!;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        FitSyncDbContext = Services.CreateScope().ServiceProvider.GetRequiredService<FitSyncDbContext>();
        _dbConnection = FitSyncDbContext.Database.GetDbConnection();
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
    }

    public new Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected override async void Dispose(bool disposing)
    {
        if (disposing)
        {
            await _dbConnection.CloseAsync();
            await _dbConnection.DisposeAsync();

            await _postgreSqlContainer.StopAsync();
            await _postgreSqlContainer.DisposeAsync();
        }
        base.Dispose(disposing);
    }

    public async Task ResetDatabase()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<FitSyncDbContext>();
            services.AddDbContext<FitSyncDbContext>(options =>
            {
                options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
            });
        });
    }
}
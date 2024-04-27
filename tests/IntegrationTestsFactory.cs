using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Api.Tests;

public sealed class IntegrationTestsFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:Sqlite", "DataSource=test.db;Cache=Shared");
        builder.ConfigureTestServices(serviceCollection =>
        {
            using var scope = serviceCollection
                .BuildServiceProvider()
                .CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<AppDbContext>>();
            var context = services.GetService<AppDbContext>();

            logger.LogInformation("[INTEGRATION TESTS] starting database migration...");

            context.Database.EnsureDeleted();
            context.Database.Migrate();

            logger.LogInformation("[INTEGRATION TESTS] database migration completed.");
        });
    }
}

[CollectionDefinition(nameof(CollectionIntegrationTests))]
public sealed class CollectionIntegrationTests : ICollectionFixture<IntegrationTestsFactory> { }

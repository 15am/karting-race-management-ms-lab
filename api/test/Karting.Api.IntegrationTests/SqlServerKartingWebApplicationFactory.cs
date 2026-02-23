using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Karting.Api.IntegrationTests;

internal sealed class SqlServerKartingWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            var values = new Dictionary<string, string?>
            {
                ["Database:Provider"] = "SqlServer",
                ["ConnectionStrings:SqlServer"] = connectionString,
            };

            configurationBuilder.AddInMemoryCollection(values);
        });
    }
}

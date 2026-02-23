namespace Karting.Api.IntegrationTests;

internal static class SqlServerIntegrationSettings
{
    private const string EnabledVariable = "KARTING_SQL_INTEGRATION";
    private const string ConnectionStringVariable = "KARTING_SQL_CONNECTION_STRING";

    internal static bool IsEnabled => string.Equals(
        Environment.GetEnvironmentVariable(EnabledVariable),
        "true",
        StringComparison.OrdinalIgnoreCase);

    internal static string GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable);
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        throw new InvalidOperationException($"{ConnectionStringVariable} is required when SQL integration is enabled.");
    }
}

using Xunit;

namespace Karting.Api.IntegrationTests;

/// <summary>
/// Marks SQL Server integration tests skipped unless the opt-in setting is enabled.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class SqlServerFactAttribute : FactAttribute
{
    public SqlServerFactAttribute()
    {
        if (!SqlServerIntegrationSettings.IsEnabled)
        {
            Skip = "SQL Server integration tests require KARTING_SQL_INTEGRATION=true.";
        }
    }
}

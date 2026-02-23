using System.Text;
using Microsoft.Data.SqlClient;

namespace Karting.Api.IntegrationTests;

internal static class SqlServerScriptRunner
{
    internal static async Task ApplySqlAssetsAsync(string connectionString, CancellationToken cancellationToken)
    {
        var sqlRoot = ResolveSqlRootDirectory();
        var scripts = GetOrderedScriptPaths(sqlRoot);
        await using var connection = new SqlConnection(ToMasterConnection(connectionString));
        await connection.OpenAsync(cancellationToken);
        foreach (var scriptPath in scripts)
        {
            var scriptText = await File.ReadAllTextAsync(scriptPath, cancellationToken);
            foreach (var batch in SplitBatches(scriptText))
            {
                await ExecuteBatchAsync(connection, batch, cancellationToken);
            }
        }
    }

    internal static async Task ResetRaceDataAsync(string connectionString, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(EnsureTargetDatabase(connectionString));
        await connection.OpenAsync(cancellationToken);
        const string resetSql = """
            DELETE FROM dbo.LapTimes;
            DELETE FROM dbo.Races;
            DELETE FROM dbo.Drivers;
            """;
        await ExecuteBatchAsync(connection, resetSql, cancellationToken);
    }

    private static string ResolveSqlRootDirectory()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "deployment", "sql");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
            current = current.Parent;
        }
        throw new DirectoryNotFoundException("Could not locate deployment/sql from test output directory.");
    }

    private static IReadOnlyList<string> GetOrderedScriptPaths(string sqlRoot)
    {
        var schemaPaths = Directory.GetFiles(Path.Combine(sqlRoot, "schema"), "*.sql").OrderBy(path => path);
        var procedurePaths = Directory.GetFiles(Path.Combine(sqlRoot, "stored-procedures"), "*.sql").OrderBy(path => path);
        return schemaPaths.Concat(procedurePaths).ToList();
    }

    private static string ToMasterConnection(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" };
        return builder.ConnectionString;
    }

    private static string EnsureTargetDatabase(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        if (!string.IsNullOrWhiteSpace(builder.InitialCatalog))
        {
            return builder.ConnectionString;
        }
        builder.InitialCatalog = "KartingRaceManagement";
        return builder.ConnectionString;
    }

    private static IReadOnlyList<string> SplitBatches(string scriptText)
    {
        var batches = new List<string>();
        var builder = new StringBuilder();
        var lines = scriptText.Replace("\r\n", "\n").Split('\n');
        foreach (var rawLine in lines)
        {
            if (string.Equals(rawLine.Trim(), "GO", StringComparison.OrdinalIgnoreCase))
            {
                AddBatchIfPresent(builder, batches);
                continue;
            }
            builder.AppendLine(rawLine);
        }
        AddBatchIfPresent(builder, batches);
        return batches;
    }

    private static void AddBatchIfPresent(StringBuilder builder, ICollection<string> batches)
    {
        var text = builder.ToString().Trim();
        if (string.IsNullOrWhiteSpace(text))
        {
            builder.Clear();
            return;
        }
        batches.Add(text);
        builder.Clear();
    }

    private static async Task ExecuteBatchAsync(SqlConnection connection, string sql, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandTimeout = 120;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}

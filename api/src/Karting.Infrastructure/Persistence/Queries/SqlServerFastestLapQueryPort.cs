using System.Data;
using Karting.Application.Abstractions.Queries;
using Karting.Application.Features.Races.Common;
using Karting.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Karting.Infrastructure.Persistence.Queries;

/// <summary>
/// Represents the SqlServerFastestLapQueryPort class.
/// </summary>
public sealed class SqlServerFastestLapQueryPort(KartingDbContext dbContext) : IFastestLapQueryPort
{
    /// <summary>
    /// Executes GetFastestLapAsync.
    /// </summary>
    /// <param name="raceId">The raceId value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public async Task<FastestLapItem?> GetFastestLapAsync(RaceId raceId, CancellationToken cancellationToken)
    {
        var provider = dbContext.Database.ProviderName ?? string.Empty;
        if (!provider.Contains("SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            return await GetFallbackAsync(raceId, cancellationToken);
        }

        var connection = dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = "sp_GetFastestLap";
        command.CommandType = CommandType.StoredProcedure;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@RaceId";
        parameter.Value = raceId.Value;
        command.Parameters.Add(parameter);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var hasRow = await reader.ReadAsync(cancellationToken);
        if (!hasRow)
        {
            return null;
        }

        return new FastestLapItem(
            raceId.Value,
            GetGuid(reader, "DriverId"),
            GetString(reader, "DriverName"),
            GetInt(reader, "LapNumber"),
            GetInt(reader, "TimeMilliseconds"));
    }

    private async Task<FastestLapItem?> GetFallbackAsync(RaceId raceId, CancellationToken cancellationToken)
    {
        var item = await dbContext.LapTimes
            .AsNoTracking()
            .Where(x => x.RaceId == raceId.Value)
            .Join(dbContext.Drivers.AsNoTracking(), lap => lap.DriverId, driver => driver.Id, (lap, driver) => new
            {
                lap.RaceId,
                lap.DriverId,
                DriverName = driver.Name,
                lap.LapNumber,
                lap.TimeMilliseconds,
            })
            .OrderBy(x => x.TimeMilliseconds)
            .ThenBy(x => x.LapNumber)
            .Select(x => new FastestLapItem(x.RaceId, x.DriverId, x.DriverName, x.LapNumber, x.TimeMilliseconds))
            .FirstOrDefaultAsync(cancellationToken);

        return item;
    }

    private static int GetInt(IDataRecord record, string column) => Convert.ToInt32(record[column]);

    private static Guid GetGuid(IDataRecord record, string column) => Guid.Parse(record[column].ToString()!);

    private static string GetString(IDataRecord record, string column) => record[column].ToString() ?? string.Empty;
}

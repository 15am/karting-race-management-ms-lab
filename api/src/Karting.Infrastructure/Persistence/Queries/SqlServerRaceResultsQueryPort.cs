using System.Data;
using Karting.Application.Abstractions.Queries;
using Karting.Application.Features.Races.Common;
using Karting.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Karting.Infrastructure.Persistence.Queries;

public sealed class SqlServerRaceResultsQueryPort(KartingDbContext dbContext) : IRaceResultsQueryPort
{
    public async Task<IReadOnlyList<RaceResultItem>> GetRaceResultsAsync(RaceId raceId, CancellationToken cancellationToken)
    {
        var provider = dbContext.Database.ProviderName ?? string.Empty;
        if (!provider.Contains("SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            return await GetFallbackAsync(raceId, cancellationToken);
        }

        var items = new List<RaceResultItem>();
        var connection = dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = "sp_GetRaceResults";
        command.CommandType = CommandType.StoredProcedure;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@RaceId";
        parameter.Value = raceId.Value;
        command.Parameters.Add(parameter);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            items.Add(new RaceResultItem(
                GetInt(reader, "Position"),
                GetGuid(reader, "DriverId"),
                GetString(reader, "DriverName"),
                GetInt(reader, "TotalTimeMilliseconds"),
                GetInt(reader, "LapCount")));
        }

        return items;
    }

    private async Task<IReadOnlyList<RaceResultItem>> GetFallbackAsync(RaceId raceId, CancellationToken cancellationToken)
    {
        var aggregates = await dbContext.LapTimes
            .AsNoTracking()
            .Where(x => x.RaceId == raceId.Value)
            .Join(dbContext.Drivers.AsNoTracking(), lap => lap.DriverId, driver => driver.Id, (lap, driver) => new
            {
                lap.DriverId,
                DriverName = driver.Name,
                lap.TimeMilliseconds,
            })
            .GroupBy(x => new { x.DriverId, x.DriverName })
            .Select(group => new
            {
                group.Key.DriverId,
                group.Key.DriverName,
                Total = group.Sum(x => x.TimeMilliseconds),
                LapCount = group.Count(),
            })
            .OrderBy(x => x.Total)
            .ThenBy(x => x.DriverName)
            .ToListAsync(cancellationToken);

        var response = new List<RaceResultItem>();
        var position = 1;
        foreach (var aggregate in aggregates)
        {
            response.Add(new RaceResultItem(
                position,
                aggregate.DriverId,
                aggregate.DriverName,
                aggregate.Total,
                aggregate.LapCount));

            position += 1;
        }

        return response;
    }

    private static int GetInt(IDataRecord record, string column) => Convert.ToInt32(record[column]);

    private static Guid GetGuid(IDataRecord record, string column) => Guid.Parse(record[column].ToString()!);

    private static string GetString(IDataRecord record, string column) => record[column].ToString() ?? string.Empty;
}

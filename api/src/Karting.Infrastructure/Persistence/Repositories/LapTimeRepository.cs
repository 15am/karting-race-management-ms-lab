using Karting.Application.Abstractions.Persistence;
using Karting.Domain.Entities;
using Karting.Infrastructure.Persistence.Entities;

namespace Karting.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the LapTimeRepository class.
/// </summary>
public sealed class LapTimeRepository(KartingDbContext dbContext) : ILapTimeRepository
{
    /// <summary>
    /// Executes AddAsync.
    /// </summary>
    /// <param name="lapTime">The lapTime value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public async Task AddAsync(LapTime lapTime, CancellationToken cancellationToken)
    {
        var entity = new LapTimeEntity
        {
            Id = lapTime.Id.Value,
            RaceId = lapTime.RaceId.Value,
            DriverId = lapTime.DriverId.Value,
            LapNumber = lapTime.LapNumber.Value,
            TimeMilliseconds = lapTime.TimeMilliseconds.Value,
            CreatedAtUtc = lapTime.CreatedAtUtc,
        };

        await dbContext.LapTimes.AddAsync(entity, cancellationToken);
    }
}

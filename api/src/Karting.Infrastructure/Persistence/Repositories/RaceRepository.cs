using Karting.Application.Abstractions.Persistence;
using Karting.Domain.Entities;
using Karting.Domain.ValueObjects;
using Karting.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karting.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the RaceRepository class.
/// </summary>
public sealed class RaceRepository(KartingDbContext dbContext) : IRaceRepository
{
    /// <summary>
    /// Executes AddAsync.
    /// </summary>
    /// <param name="race">The race value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public async Task AddAsync(Race race, CancellationToken cancellationToken)
    {
        var entity = new RaceEntity
        {
            Id = race.Id.Value,
            Name = race.Name.Value,
            DateUtc = race.DateUtc,
            Status = race.Status.ToString(),
        };

        await dbContext.Races.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Executes ExistsAsync.
    /// </summary>
    /// <param name="raceId">The raceId value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public Task<bool> ExistsAsync(RaceId raceId, CancellationToken cancellationToken)
    {
        return dbContext.Races.AnyAsync(x => x.Id == raceId.Value, cancellationToken);
    }
}

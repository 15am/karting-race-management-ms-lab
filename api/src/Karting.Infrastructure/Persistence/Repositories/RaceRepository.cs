using Karting.Application.Abstractions.Persistence;
using Karting.Domain.Entities;
using Karting.Domain.ValueObjects;
using Karting.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karting.Infrastructure.Persistence.Repositories;

public sealed class RaceRepository(KartingDbContext dbContext) : IRaceRepository
{
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

    public Task<bool> ExistsAsync(RaceId raceId, CancellationToken cancellationToken)
    {
        return dbContext.Races.AnyAsync(x => x.Id == raceId.Value, cancellationToken);
    }
}

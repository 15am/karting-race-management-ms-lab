using Karting.Domain.Entities;
using Karting.Domain.ValueObjects;

namespace Karting.Application.Abstractions.Persistence;

public interface IRaceRepository
{
    Task AddAsync(Race race, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(RaceId raceId, CancellationToken cancellationToken);
}

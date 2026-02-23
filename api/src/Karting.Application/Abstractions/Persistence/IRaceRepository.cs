using Karting.Domain.Entities;
using Karting.Domain.ValueObjects;

namespace Karting.Application.Abstractions.Persistence;

/// <summary>
/// Represents the IRaceRepository interface.
/// </summary>
public interface IRaceRepository
{
    /// <summary>
    /// Adds a race entity to persistence.
    /// </summary>
    /// <param name="race">The race entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the add operation is queued.</returns>
    Task AddAsync(Race race, CancellationToken cancellationToken);

    /// <summary>
    /// Checks whether a race exists.
    /// </summary>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True when the race exists; otherwise false.</returns>
    Task<bool> ExistsAsync(RaceId raceId, CancellationToken cancellationToken);
}

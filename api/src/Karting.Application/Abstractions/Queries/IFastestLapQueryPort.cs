using Karting.Application.Features.Races.Common;
using Karting.Domain.ValueObjects;

namespace Karting.Application.Abstractions.Queries;

/// <summary>
/// Represents the IFastestLapQueryPort interface.
/// </summary>
public interface IFastestLapQueryPort
{
    /// <summary>
    /// Gets the fastest lap for a race.
    /// </summary>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The fastest lap result when available; otherwise null.</returns>
    Task<FastestLapItem?> GetFastestLapAsync(RaceId raceId, CancellationToken cancellationToken);
}

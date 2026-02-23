using Karting.Application.Features.Races.Common;
using Karting.Domain.ValueObjects;

namespace Karting.Application.Abstractions.Queries;

/// <summary>
/// Represents the IRaceResultsQueryPort interface.
/// </summary>
public interface IRaceResultsQueryPort
{
    /// <summary>
    /// Gets race result aggregates for a race.
    /// </summary>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The ordered race-result items.</returns>
    Task<IReadOnlyList<RaceResultItem>> GetRaceResultsAsync(RaceId raceId, CancellationToken cancellationToken);
}

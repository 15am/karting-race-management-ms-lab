using Karting.Application.Features.Races.Common;
using Karting.Domain.ValueObjects;

namespace Karting.Application.Abstractions.Queries;

public interface IRaceResultsQueryPort
{
    Task<IReadOnlyList<RaceResultItem>> GetRaceResultsAsync(RaceId raceId, CancellationToken cancellationToken);
}

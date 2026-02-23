using Karting.Application.Features.Races.Common;
using Karting.Domain.ValueObjects;

namespace Karting.Application.Abstractions.Queries;

public interface IFastestLapQueryPort
{
    Task<FastestLapItem?> GetFastestLapAsync(RaceId raceId, CancellationToken cancellationToken);
}

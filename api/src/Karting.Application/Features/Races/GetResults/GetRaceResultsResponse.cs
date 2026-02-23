using Karting.Application.Features.Races.Common;

namespace Karting.Application.Features.Races.GetResults;

/// <summary>
/// Represents the GetRaceResultsResponse record.
/// </summary>
public sealed record GetRaceResultsResponse(IReadOnlyList<RaceResultItem> Results);

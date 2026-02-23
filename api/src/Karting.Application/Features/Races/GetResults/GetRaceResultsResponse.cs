using Karting.Application.Features.Races.Common;

namespace Karting.Application.Features.Races.GetResults;

public sealed record GetRaceResultsResponse(IReadOnlyList<RaceResultItem> Results);

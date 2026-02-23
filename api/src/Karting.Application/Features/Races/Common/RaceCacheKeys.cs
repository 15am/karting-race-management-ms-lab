namespace Karting.Application.Features.Races.Common;

public static class RaceCacheKeys
{
    public static string Results(Guid raceId) => $"race:{raceId}:results";

    public static string FastestLap(Guid raceId) => $"race:{raceId}:fastestlap";
}

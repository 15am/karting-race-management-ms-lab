namespace Karting.Application.Features.Races.Common;

/// <summary>
/// Represents the RaceCacheKeys class.
/// </summary>
public static class RaceCacheKeys
{
    /// <summary>
    /// Executes Results.
    /// </summary>
    /// <param name="raceId">The raceId value.</param>
    /// <returns>The operation result.</returns>
    public static string Results(Guid raceId) => $"race:{raceId}:results";

    /// <summary>
    /// Executes FastestLap.
    /// </summary>
    /// <param name="raceId">The raceId value.</param>
    /// <returns>The operation result.</returns>
    public static string FastestLap(Guid raceId) => $"race:{raceId}:fastestlap";
}

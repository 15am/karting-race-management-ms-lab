namespace Karting.Application.Features.Races.GetFastestLap;

/// <summary>
/// Represents the GetFastestLapResponse record.
/// </summary>
public sealed record GetFastestLapResponse(
    Guid RaceId,
    Guid DriverId,
    string DriverName,
    int LapNumber,
    int TimeMilliseconds);

namespace Karting.Application.Features.Races.Common;

/// <summary>
/// Represents the FastestLapItem record.
/// </summary>
public sealed record FastestLapItem(
    Guid RaceId,
    Guid DriverId,
    string DriverName,
    int LapNumber,
    int TimeMilliseconds);

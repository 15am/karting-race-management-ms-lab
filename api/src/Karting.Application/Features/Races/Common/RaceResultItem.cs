namespace Karting.Application.Features.Races.Common;

/// <summary>
/// Represents the RaceResultItem record.
/// </summary>
public sealed record RaceResultItem(
    int Position,
    Guid DriverId,
    string DriverName,
    int TotalTimeMilliseconds,
    int LapCount);

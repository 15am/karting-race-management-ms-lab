namespace Karting.Application.Features.LapTimes.Create;

/// <summary>
/// Represents the CreateLapTimeResponse record.
/// </summary>
public sealed record CreateLapTimeResponse(
    Guid Id,
    Guid RaceId,
    Guid DriverId,
    int LapNumber,
    int TimeMilliseconds,
    DateTime CreatedAtUtc);

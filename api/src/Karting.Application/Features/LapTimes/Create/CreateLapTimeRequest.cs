namespace Karting.Application.Features.LapTimes.Create;

/// <summary>
/// Represents the CreateLapTimeRequest record.
/// </summary>
public sealed record CreateLapTimeRequest(Guid RaceId, Guid DriverId, int LapNumber, int TimeMilliseconds);

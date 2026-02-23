namespace Karting.Application.Features.LapTimes.Create;

public sealed record CreateLapTimeResponse(
    Guid Id,
    Guid RaceId,
    Guid DriverId,
    int LapNumber,
    int TimeMilliseconds,
    DateTime CreatedAtUtc);

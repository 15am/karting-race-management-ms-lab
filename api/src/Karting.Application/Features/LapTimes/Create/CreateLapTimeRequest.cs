namespace Karting.Application.Features.LapTimes.Create;

public sealed record CreateLapTimeRequest(Guid RaceId, Guid DriverId, int LapNumber, int TimeMilliseconds);

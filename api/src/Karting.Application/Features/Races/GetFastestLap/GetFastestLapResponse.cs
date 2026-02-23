namespace Karting.Application.Features.Races.GetFastestLap;

public sealed record GetFastestLapResponse(
    Guid RaceId,
    Guid DriverId,
    string DriverName,
    int LapNumber,
    int TimeMilliseconds);

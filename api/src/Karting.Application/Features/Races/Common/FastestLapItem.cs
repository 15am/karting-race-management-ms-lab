namespace Karting.Application.Features.Races.Common;

public sealed record FastestLapItem(
    Guid RaceId,
    Guid DriverId,
    string DriverName,
    int LapNumber,
    int TimeMilliseconds);

namespace Karting.Application.Features.Races.Common;

public sealed record RaceResultItem(
    int Position,
    Guid DriverId,
    string DriverName,
    int TotalTimeMilliseconds,
    int LapCount);

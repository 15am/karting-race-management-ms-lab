namespace Karting.Application.Features.Races.Create;

public sealed record CreateRaceRequest(string Name, DateTime DateUtc, string Status);

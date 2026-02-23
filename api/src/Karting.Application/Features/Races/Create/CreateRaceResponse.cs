namespace Karting.Application.Features.Races.Create;

public sealed record CreateRaceResponse(Guid Id, string Name, DateTime DateUtc, string Status);

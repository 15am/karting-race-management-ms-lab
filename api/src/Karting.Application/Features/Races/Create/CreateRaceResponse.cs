namespace Karting.Application.Features.Races.Create;

/// <summary>
/// Represents the CreateRaceResponse record.
/// </summary>
public sealed record CreateRaceResponse(Guid Id, string Name, DateTime DateUtc, string Status);

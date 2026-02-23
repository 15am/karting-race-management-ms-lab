namespace Karting.Application.Features.Races.Create;

/// <summary>
/// Represents the CreateRaceRequest record.
/// </summary>
public sealed record CreateRaceRequest(string Name, DateTime DateUtc, string Status);

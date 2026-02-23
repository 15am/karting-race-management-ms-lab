namespace Karting.Application.Features.Drivers.Create;

/// <summary>
/// Represents the CreateDriverResponse record.
/// </summary>
public sealed record CreateDriverResponse(Guid Id, string Name, DateTime CreatedAtUtc);

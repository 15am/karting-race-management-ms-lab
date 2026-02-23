namespace Karting.Application.Features.Drivers.Create;

public sealed record CreateDriverResponse(Guid Id, string Name, DateTime CreatedAtUtc);

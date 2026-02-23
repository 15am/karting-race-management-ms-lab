namespace Karting.Infrastructure.Persistence.Entities;

public sealed class LapTimeEntity
{
    public Guid Id { get; set; }

    public Guid RaceId { get; set; }

    public Guid DriverId { get; set; }

    public int LapNumber { get; set; }

    public int TimeMilliseconds { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public RaceEntity? Race { get; set; }

    public DriverEntity? Driver { get; set; }
}

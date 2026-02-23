namespace Karting.Infrastructure.Persistence.Entities;

public sealed class DriverEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<LapTimeEntity> LapTimes { get; set; } = [];
}

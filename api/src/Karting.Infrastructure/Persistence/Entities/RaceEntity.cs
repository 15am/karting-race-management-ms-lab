namespace Karting.Infrastructure.Persistence.Entities;

public sealed class RaceEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateUtc { get; set; }

    public string Status { get; set; } = string.Empty;

    public ICollection<LapTimeEntity> LapTimes { get; set; } = [];
}

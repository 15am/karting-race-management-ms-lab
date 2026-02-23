namespace Karting.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents the RaceEntity class.
/// </summary>
public sealed class RaceEntity
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DateTime DateUtc { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public ICollection<LapTimeEntity> LapTimes { get; set; } = [];
}

namespace Karting.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents the DriverEntity class.
/// </summary>
public sealed class DriverEntity
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
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public ICollection<LapTimeEntity> LapTimes { get; set; } = [];
}

namespace Karting.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents the LapTimeEntity class.
/// </summary>
public sealed class LapTimeEntity
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public Guid RaceId { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public Guid DriverId { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public int LapNumber { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public int TimeMilliseconds { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public RaceEntity? Race { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DriverEntity? Driver { get; set; }
}

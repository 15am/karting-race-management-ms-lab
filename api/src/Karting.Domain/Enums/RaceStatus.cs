namespace Karting.Domain.Enums;

/// <summary>
/// Represents the RaceStatus enum.
/// </summary>
public enum RaceStatus
{
    /// <summary>
    /// The race is scheduled and has not started.
    /// </summary>
    Scheduled = 1,
    /// <summary>
    /// The race is currently in progress.
    /// </summary>
    InProgress = 2,
    /// <summary>
    /// The race has been completed.
    /// </summary>
    Completed = 3
}

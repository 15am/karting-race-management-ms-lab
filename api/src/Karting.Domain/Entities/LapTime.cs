using Karting.Domain.Exceptions;
using Karting.Domain.ValueObjects;

namespace Karting.Domain.Entities;

/// <summary>
/// Represents the LapTime class.
/// </summary>
public sealed class LapTime
{
    private LapTime(
        LapTimeId id,
        RaceId raceId,
        DriverId driverId,
        LapNumber lapNumber,
        TimeMilliseconds timeMilliseconds,
        DateTime createdAtUtc)
    {
        if (createdAtUtc.Kind != DateTimeKind.Utc)
        {
            throw new DomainRuleException("LapTime created date must be UTC.");
        }

        Id = id;
        RaceId = raceId;
        DriverId = driverId;
        LapNumber = lapNumber;
        TimeMilliseconds = timeMilliseconds;
        CreatedAtUtc = createdAtUtc;
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public LapTimeId Id { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public RaceId RaceId { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DriverId DriverId { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public LapNumber LapNumber { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public TimeMilliseconds TimeMilliseconds { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DateTime CreatedAtUtc { get; }

    /// <summary>
    /// Executes Create.
    /// </summary>
    /// <param name="raceId">The raceId value.</param>
    /// <param name="driverId">The driverId value.</param>
    /// <param name="lapNumber">The lapNumber value.</param>
    /// <param name="timeMilliseconds">The timeMilliseconds value.</param>
    /// <param name="createdAtUtc">The createdAtUtc value.</param>
    /// <returns>The operation result.</returns>
    public static LapTime Create(Guid raceId, Guid driverId, int lapNumber, int timeMilliseconds, DateTime? createdAtUtc = null)
    {
        var created = createdAtUtc ?? DateTime.UtcNow;
        return new LapTime(
            LapTimeId.New(),
            RaceId.From(raceId),
            DriverId.From(driverId),
            LapNumber.Create(lapNumber),
            TimeMilliseconds.Create(timeMilliseconds),
            created);
    }

    /// <summary>
    /// Represents a publicly exposed member.
    /// </summary>
    public static LapTime Rehydrate(
        Guid id,
        Guid raceId,
        Guid driverId,
        int lapNumber,
        int timeMilliseconds,
        DateTime createdAtUtc)
    {
        return new LapTime(
            LapTimeId.From(id),
            RaceId.From(raceId),
            DriverId.From(driverId),
            LapNumber.Create(lapNumber),
            TimeMilliseconds.Create(timeMilliseconds),
            createdAtUtc);
    }
}

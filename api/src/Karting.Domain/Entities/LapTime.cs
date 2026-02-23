using Karting.Domain.Exceptions;
using Karting.Domain.ValueObjects;

namespace Karting.Domain.Entities;

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

    public LapTimeId Id { get; }

    public RaceId RaceId { get; }

    public DriverId DriverId { get; }

    public LapNumber LapNumber { get; }

    public TimeMilliseconds TimeMilliseconds { get; }

    public DateTime CreatedAtUtc { get; }

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

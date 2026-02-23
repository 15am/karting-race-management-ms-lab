using Karting.Domain.Enums;
using Karting.Domain.Exceptions;
using Karting.Domain.ValueObjects;

namespace Karting.Domain.Entities;

public sealed class Race
{
    private Race(RaceId id, RaceName name, DateTime dateUtc, RaceStatus status)
    {
        if (dateUtc.Kind != DateTimeKind.Utc)
        {
            throw new DomainRuleException("Race date must be UTC.");
        }

        Id = id;
        Name = name;
        DateUtc = dateUtc;
        Status = status;
    }

    public RaceId Id { get; }

    public RaceName Name { get; }

    public DateTime DateUtc { get; }

    public RaceStatus Status { get; }

    public static Race Create(string name, DateTime dateUtc, RaceStatus status)
    {
        return new Race(RaceId.New(), RaceName.Create(name), dateUtc, status);
    }

    public static Race Rehydrate(Guid id, string name, DateTime dateUtc, RaceStatus status)
    {
        return new Race(RaceId.From(id), RaceName.Create(name), dateUtc, status);
    }
}

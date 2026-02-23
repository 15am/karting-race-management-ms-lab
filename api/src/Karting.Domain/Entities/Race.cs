using Karting.Domain.Enums;
using Karting.Domain.Exceptions;
using Karting.Domain.ValueObjects;

namespace Karting.Domain.Entities;

/// <summary>
/// Represents the Race class.
/// </summary>
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

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public RaceId Id { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public RaceName Name { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DateTime DateUtc { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public RaceStatus Status { get; }

    /// <summary>
    /// Executes Create.
    /// </summary>
    /// <param name="name">The name value.</param>
    /// <param name="dateUtc">The dateUtc value.</param>
    /// <param name="status">The status value.</param>
    /// <returns>The operation result.</returns>
    public static Race Create(string name, DateTime dateUtc, RaceStatus status)
    {
        return new Race(RaceId.New(), RaceName.Create(name), dateUtc, status);
    }

    /// <summary>
    /// Executes Rehydrate.
    /// </summary>
    /// <param name="id">The id value.</param>
    /// <param name="name">The name value.</param>
    /// <param name="dateUtc">The dateUtc value.</param>
    /// <param name="status">The status value.</param>
    /// <returns>The operation result.</returns>
    public static Race Rehydrate(Guid id, string name, DateTime dateUtc, RaceStatus status)
    {
        return new Race(RaceId.From(id), RaceName.Create(name), dateUtc, status);
    }
}

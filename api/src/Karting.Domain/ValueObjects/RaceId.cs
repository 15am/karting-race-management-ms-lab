using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

/// <summary>
/// Represents the struct record.
/// </summary>
public readonly record struct RaceId(Guid Value)
{
    /// <summary>
    /// Executes New.
    /// </summary>
    /// <returns>The operation result.</returns>
    public static RaceId New() => new(Guid.NewGuid());

    /// <summary>
    /// Executes From.
    /// </summary>
    /// <param name="value">The value value.</param>
    /// <returns>The operation result.</returns>
    public static RaceId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainRuleException("RaceId cannot be empty.");
        }

        return new RaceId(value);
    }

    /// <summary>
    /// Executes ToString.
    /// </summary>
    /// <returns>The operation result.</returns>
    public override string ToString() => Value.ToString();
}

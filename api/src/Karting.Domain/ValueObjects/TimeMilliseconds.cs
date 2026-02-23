using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

/// <summary>
/// Represents the struct record.
/// </summary>
public readonly record struct TimeMilliseconds(int Value)
{
    /// <summary>
    /// Executes Create.
    /// </summary>
    /// <param name="value">The value value.</param>
    /// <returns>The operation result.</returns>
    public static TimeMilliseconds Create(int value)
    {
        if (value <= 0)
        {
            throw new DomainRuleException("Lap time must be greater than zero.");
        }

        return new TimeMilliseconds(value);
    }
}

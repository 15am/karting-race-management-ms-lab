using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

/// <summary>
/// Represents the struct record.
/// </summary>
public readonly record struct LapNumber(int Value)
{
    /// <summary>
    /// Executes Create.
    /// </summary>
    /// <param name="value">The value value.</param>
    /// <returns>The operation result.</returns>
    public static LapNumber Create(int value)
    {
        if (value <= 0)
        {
            throw new DomainRuleException("Lap number must be greater than zero.");
        }

        return new LapNumber(value);
    }
}

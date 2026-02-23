using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

/// <summary>
/// Represents the struct record.
/// </summary>
public readonly record struct LapTimeId(Guid Value)
{
    /// <summary>
    /// Executes New.
    /// </summary>
    /// <returns>The operation result.</returns>
    public static LapTimeId New() => new(Guid.NewGuid());

    /// <summary>
    /// Executes From.
    /// </summary>
    /// <param name="value">The value value.</param>
    /// <returns>The operation result.</returns>
    public static LapTimeId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainRuleException("LapTimeId cannot be empty.");
        }

        return new LapTimeId(value);
    }

    /// <summary>
    /// Executes ToString.
    /// </summary>
    /// <returns>The operation result.</returns>
    public override string ToString() => Value.ToString();
}

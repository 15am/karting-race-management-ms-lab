using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

/// <summary>
/// Represents the struct record.
/// </summary>
public readonly record struct RaceName(string Value)
{
    /// <summary>
    /// Executes Create.
    /// </summary>
    /// <param name="value">The value value.</param>
    /// <returns>The operation result.</returns>
    public static RaceName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException("Race name is required.");
        }

        var normalized = value.Trim();
        if (normalized.Length > 100)
        {
            throw new DomainRuleException("Race name cannot exceed 100 characters.");
        }

        return new RaceName(normalized);
    }

    /// <summary>
    /// Executes ToString.
    /// </summary>
    /// <returns>The operation result.</returns>
    public override string ToString() => Value;
}

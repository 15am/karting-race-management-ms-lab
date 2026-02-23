using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

/// <summary>
/// Represents the struct record.
/// </summary>
public readonly record struct DriverName(string Value)
{
    /// <summary>
    /// Executes Create.
    /// </summary>
    /// <param name="value">The value value.</param>
    /// <returns>The operation result.</returns>
    public static DriverName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException("Driver name is required.");
        }

        var normalized = value.Trim();
        if (normalized.Length > 80)
        {
            throw new DomainRuleException("Driver name cannot exceed 80 characters.");
        }

        return new DriverName(normalized);
    }

    /// <summary>
    /// Executes ToString.
    /// </summary>
    /// <returns>The operation result.</returns>
    public override string ToString() => Value;
}

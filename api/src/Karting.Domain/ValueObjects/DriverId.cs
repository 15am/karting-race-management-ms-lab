using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

/// <summary>
/// Represents the struct record.
/// </summary>
public readonly record struct DriverId(Guid Value)
{
    /// <summary>
    /// Executes New.
    /// </summary>
    /// <returns>The operation result.</returns>
    public static DriverId New() => new(Guid.NewGuid());

    /// <summary>
    /// Executes From.
    /// </summary>
    /// <param name="value">The value value.</param>
    /// <returns>The operation result.</returns>
    public static DriverId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainRuleException("DriverId cannot be empty.");
        }

        return new DriverId(value);
    }

    /// <summary>
    /// Executes ToString.
    /// </summary>
    /// <returns>The operation result.</returns>
    public override string ToString() => Value.ToString();
}

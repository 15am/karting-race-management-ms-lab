using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

public readonly record struct DriverName(string Value)
{
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

    public override string ToString() => Value;
}

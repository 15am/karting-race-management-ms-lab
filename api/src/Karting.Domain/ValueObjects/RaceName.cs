using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

public readonly record struct RaceName(string Value)
{
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

    public override string ToString() => Value;
}

using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

public readonly record struct RaceId(Guid Value)
{
    public static RaceId New() => new(Guid.NewGuid());

    public static RaceId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainRuleException("RaceId cannot be empty.");
        }

        return new RaceId(value);
    }

    public override string ToString() => Value.ToString();
}

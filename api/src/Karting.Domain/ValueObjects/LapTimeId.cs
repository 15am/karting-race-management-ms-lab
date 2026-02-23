using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

public readonly record struct LapTimeId(Guid Value)
{
    public static LapTimeId New() => new(Guid.NewGuid());

    public static LapTimeId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainRuleException("LapTimeId cannot be empty.");
        }

        return new LapTimeId(value);
    }

    public override string ToString() => Value.ToString();
}

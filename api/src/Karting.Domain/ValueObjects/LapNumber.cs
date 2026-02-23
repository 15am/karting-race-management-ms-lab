using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

public readonly record struct LapNumber(int Value)
{
    public static LapNumber Create(int value)
    {
        if (value <= 0)
        {
            throw new DomainRuleException("Lap number must be greater than zero.");
        }

        return new LapNumber(value);
    }
}

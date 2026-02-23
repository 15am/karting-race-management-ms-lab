using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

public readonly record struct TimeMilliseconds(int Value)
{
    public static TimeMilliseconds Create(int value)
    {
        if (value <= 0)
        {
            throw new DomainRuleException("Lap time must be greater than zero.");
        }

        return new TimeMilliseconds(value);
    }
}

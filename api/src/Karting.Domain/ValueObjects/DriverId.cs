using Karting.Domain.Exceptions;

namespace Karting.Domain.ValueObjects;

public readonly record struct DriverId(Guid Value)
{
    public static DriverId New() => new(Guid.NewGuid());

    public static DriverId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainRuleException("DriverId cannot be empty.");
        }

        return new DriverId(value);
    }

    public override string ToString() => Value.ToString();
}

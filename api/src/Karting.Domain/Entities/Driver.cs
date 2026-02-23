using Karting.Domain.Exceptions;
using Karting.Domain.ValueObjects;

namespace Karting.Domain.Entities;

public sealed class Driver
{
    private Driver(DriverId id, DriverName name, DateTime createdAtUtc)
    {
        if (createdAtUtc.Kind != DateTimeKind.Utc)
        {
            throw new DomainRuleException("Driver created date must be UTC.");
        }

        Id = id;
        Name = name;
        CreatedAtUtc = createdAtUtc;
    }

    public DriverId Id { get; }

    public DriverName Name { get; }

    public DateTime CreatedAtUtc { get; }

    public static Driver Create(string name, DateTime? createdAtUtc = null)
    {
        var created = createdAtUtc ?? DateTime.UtcNow;
        return new Driver(DriverId.New(), DriverName.Create(name), created);
    }

    public static Driver Rehydrate(Guid id, string name, DateTime createdAtUtc)
    {
        return new Driver(DriverId.From(id), DriverName.Create(name), createdAtUtc);
    }
}

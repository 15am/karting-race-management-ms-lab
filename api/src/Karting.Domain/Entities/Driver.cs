using Karting.Domain.Exceptions;
using Karting.Domain.ValueObjects;

namespace Karting.Domain.Entities;

/// <summary>
/// Represents the Driver class.
/// </summary>
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

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DriverId Id { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DriverName Name { get; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public DateTime CreatedAtUtc { get; }

    /// <summary>
    /// Executes Create.
    /// </summary>
    /// <param name="name">The name value.</param>
    /// <param name="createdAtUtc">The createdAtUtc value.</param>
    /// <returns>The operation result.</returns>
    public static Driver Create(string name, DateTime? createdAtUtc = null)
    {
        var created = createdAtUtc ?? DateTime.UtcNow;
        return new Driver(DriverId.New(), DriverName.Create(name), created);
    }

    /// <summary>
    /// Executes Rehydrate.
    /// </summary>
    /// <param name="id">The id value.</param>
    /// <param name="name">The name value.</param>
    /// <param name="createdAtUtc">The createdAtUtc value.</param>
    /// <returns>The operation result.</returns>
    public static Driver Rehydrate(Guid id, string name, DateTime createdAtUtc)
    {
        return new Driver(DriverId.From(id), DriverName.Create(name), createdAtUtc);
    }
}

using Karting.Application.Abstractions.Persistence;
using Karting.Domain.Entities;
using Karting.Domain.ValueObjects;
using Karting.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karting.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the DriverRepository class.
/// </summary>
public sealed class DriverRepository(KartingDbContext dbContext) : IDriverRepository
{
    /// <summary>
    /// Executes AddAsync.
    /// </summary>
    /// <param name="driver">The driver value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public async Task AddAsync(Driver driver, CancellationToken cancellationToken)
    {
        var entity = new DriverEntity
        {
            Id = driver.Id.Value,
            Name = driver.Name.Value,
            CreatedAtUtc = driver.CreatedAtUtc,
        };

        await dbContext.Drivers.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Executes ExistsAsync.
    /// </summary>
    /// <param name="driverId">The driverId value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public Task<bool> ExistsAsync(DriverId driverId, CancellationToken cancellationToken)
    {
        return dbContext.Drivers.AnyAsync(x => x.Id == driverId.Value, cancellationToken);
    }
}

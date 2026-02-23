using Karting.Application.Abstractions.Persistence;
using Karting.Domain.Entities;
using Karting.Domain.ValueObjects;
using Karting.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karting.Infrastructure.Persistence.Repositories;

public sealed class DriverRepository(KartingDbContext dbContext) : IDriverRepository
{
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

    public Task<bool> ExistsAsync(DriverId driverId, CancellationToken cancellationToken)
    {
        return dbContext.Drivers.AnyAsync(x => x.Id == driverId.Value, cancellationToken);
    }
}

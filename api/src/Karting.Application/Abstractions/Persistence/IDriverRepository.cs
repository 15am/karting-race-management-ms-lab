using Karting.Domain.Entities;
using Karting.Domain.ValueObjects;

namespace Karting.Application.Abstractions.Persistence;

public interface IDriverRepository
{
    Task AddAsync(Driver driver, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(DriverId driverId, CancellationToken cancellationToken);
}

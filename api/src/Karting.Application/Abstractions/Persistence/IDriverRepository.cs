using Karting.Domain.Entities;
using Karting.Domain.ValueObjects;

namespace Karting.Application.Abstractions.Persistence;

/// <summary>
/// Represents the IDriverRepository interface.
/// </summary>
public interface IDriverRepository
{
    /// <summary>
    /// Adds a driver entity to persistence.
    /// </summary>
    /// <param name="driver">The driver entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the add operation is queued.</returns>
    Task AddAsync(Driver driver, CancellationToken cancellationToken);

    /// <summary>
    /// Checks whether a driver exists.
    /// </summary>
    /// <param name="driverId">The driver identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True when the driver exists; otherwise false.</returns>
    Task<bool> ExistsAsync(DriverId driverId, CancellationToken cancellationToken);
}

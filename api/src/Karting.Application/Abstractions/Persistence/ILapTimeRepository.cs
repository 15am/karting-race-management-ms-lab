using Karting.Domain.Entities;

namespace Karting.Application.Abstractions.Persistence;

/// <summary>
/// Represents the ILapTimeRepository interface.
/// </summary>
public interface ILapTimeRepository
{
    /// <summary>
    /// Adds a lap-time entity to persistence.
    /// </summary>
    /// <param name="lapTime">The lap-time entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when the add operation is queued.</returns>
    Task AddAsync(LapTime lapTime, CancellationToken cancellationToken);
}

namespace Karting.Application.Abstractions.Persistence;

/// <summary>
/// Represents the IUnitOfWork interface.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists pending changes to the backing store.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of affected records.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

using Karting.Application.Abstractions.Persistence;

namespace Karting.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the UnitOfWork class.
/// </summary>
public sealed class UnitOfWork(KartingDbContext dbContext) : IUnitOfWork
{
    /// <summary>
    /// Executes SaveChangesAsync.
    /// </summary>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}

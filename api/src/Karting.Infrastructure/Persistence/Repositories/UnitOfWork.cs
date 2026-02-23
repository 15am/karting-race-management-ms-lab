using Karting.Application.Abstractions.Persistence;

namespace Karting.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork(KartingDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}

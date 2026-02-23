using Karting.Domain.Entities;

namespace Karting.Application.Abstractions.Persistence;

public interface ILapTimeRepository
{
    Task AddAsync(LapTime lapTime, CancellationToken cancellationToken);
}

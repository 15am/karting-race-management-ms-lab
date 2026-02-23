using Karting.Application.Abstractions.Caching;
using Karting.Application.Abstractions.Persistence;
using Karting.Application.Features.LapTimes.Create;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Karting.Application.Tests;

public sealed class CreateLapTimeHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidRequest_StoresLapAndInvalidatesCache()
    {
        var lapTimeRepository = new Mock<ILapTimeRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var cacheService = new Mock<IRaceCacheService>();

        var handler = new CreateLapTimeHandler(
            lapTimeRepository.Object,
            unitOfWork.Object,
            cacheService.Object,
            new CreateLapTimeValidator(),
            NullLogger<CreateLapTimeHandler>.Instance);

        var request = new CreateLapTimeRequest(Guid.NewGuid(), Guid.NewGuid(), 1, 53000);
        var result = await handler.HandleAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);

        lapTimeRepository.Verify(x => x.AddAsync(It.IsAny<Karting.Domain.Entities.LapTime>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        cacheService.Verify(x => x.RemoveAsync($"race:{request.RaceId}:results", It.IsAny<CancellationToken>()), Times.Once);
        cacheService.Verify(x => x.RemoveAsync($"race:{request.RaceId}:fastestlap", It.IsAny<CancellationToken>()), Times.Once);
    }
}

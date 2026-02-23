using Karting.Application.Abstractions.Caching;
using Karting.Application.Abstractions.Queries;
using Karting.Application.Features.Races.Common;
using Karting.Application.Features.Races.GetResults;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Karting.Application.Tests;

public sealed class GetRaceResultsHandlerTests
{
    [Fact]
    public async Task HandleAsync_CacheHit_ReturnsCachedPayload()
    {
        var queryPort = new Mock<IRaceResultsQueryPort>();
        var cacheService = new Mock<IRaceCacheService>();
        var cached = new List<RaceResultItem>
        {
            new(1, Guid.NewGuid(), "Driver", 50000, 1),
        };

        cacheService.Setup(x => x.GetAsync<IReadOnlyList<RaceResultItem>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cached);

        var handler = new GetRaceResultsHandler(queryPort.Object, cacheService.Object, new GetRaceResultsValidator(), NullLogger<GetRaceResultsHandler>.Instance);
        var result = await handler.HandleAsync(new GetRaceResultsRequest(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value!.Results);

        queryPort.Verify(x => x.GetRaceResultsAsync(It.IsAny<Karting.Domain.ValueObjects.RaceId>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_CacheMiss_UsesQueryPortAndCachesResponse()
    {
        var queryPort = new Mock<IRaceResultsQueryPort>();
        var cacheService = new Mock<IRaceCacheService>();

        cacheService.Setup(x => x.GetAsync<IReadOnlyList<RaceResultItem>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyList<RaceResultItem>?)null);

        queryPort.Setup(x => x.GetRaceResultsAsync(It.IsAny<Karting.Domain.ValueObjects.RaceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new RaceResultItem(1, Guid.NewGuid(), "Driver", 51000, 1)]);

        var handler = new GetRaceResultsHandler(queryPort.Object, cacheService.Object, new GetRaceResultsValidator(), NullLogger<GetRaceResultsHandler>.Instance);
        var result = await handler.HandleAsync(new GetRaceResultsRequest(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value!.Results);

        queryPort.Verify(x => x.GetRaceResultsAsync(It.IsAny<Karting.Domain.ValueObjects.RaceId>(), It.IsAny<CancellationToken>()), Times.Once);
        cacheService.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<RaceResultItem>>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

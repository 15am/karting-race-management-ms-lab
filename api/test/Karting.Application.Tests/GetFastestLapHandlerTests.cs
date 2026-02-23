using Karting.Application.Abstractions.Caching;
using Karting.Application.Abstractions.Queries;
using Karting.Application.Features.Races.GetFastestLap;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Karting.Application.Tests;

public sealed class GetFastestLapHandlerTests
{
    [Fact]
    public async Task HandleAsync_NoData_ReturnsNotFound()
    {
        var queryPort = new Mock<IFastestLapQueryPort>();
        var cacheService = new Mock<IRaceCacheService>();

        cacheService.Setup(x => x.GetAsync<Karting.Application.Features.Races.Common.FastestLapItem>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Karting.Application.Features.Races.Common.FastestLapItem?)null);

        queryPort.Setup(x => x.GetFastestLapAsync(It.IsAny<Karting.Domain.ValueObjects.RaceId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Karting.Application.Features.Races.Common.FastestLapItem?)null);

        var handler = new GetFastestLapHandler(queryPort.Object, cacheService.Object, new GetFastestLapValidator(), NullLogger<GetFastestLapHandler>.Instance);
        var result = await handler.HandleAsync(new GetFastestLapRequest(Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorCode);
    }
}

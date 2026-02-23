using Karting.Application.Abstractions.Caching;
using Karting.Application.Abstractions.Queries;
using Karting.Application.Common;
using Karting.Application.Features.Races.Common;
using Karting.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Karting.Application.Features.Races.GetFastestLap;

public sealed class GetFastestLapHandler(
    IFastestLapQueryPort fastestLapQueryPort,
    IRaceCacheService raceCacheService,
    GetFastestLapValidator validator,
    ILogger<GetFastestLapHandler> logger)
{
    public async Task<Result<GetFastestLapResponse>> HandleAsync(GetFastestLapRequest request, CancellationToken cancellationToken)
    {
        var validation = validator.Validate(request);
        if (!validation.IsSuccess)
        {
            return Result<GetFastestLapResponse>.Failure(validation.ErrorCode!, validation.ErrorMessage!);
        }

        var cacheKey = RaceCacheKeys.FastestLap(request.RaceId);
        var cached = await raceCacheService.GetAsync<FastestLapItem>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            logger.LogInformation("Fastest lap cache hit {RaceId}", request.RaceId);
            return Result<GetFastestLapResponse>.Success(ToResponse(cached));
        }

        var raceId = RaceId.From(request.RaceId);
        var fastestLap = await fastestLapQueryPort.GetFastestLapAsync(raceId, cancellationToken);
        if (fastestLap is null)
        {
            return Result<GetFastestLapResponse>.Failure(ErrorCodes.NotFound, "Fastest lap not found.");
        }

        await raceCacheService.SetAsync(cacheKey, fastestLap, TimeSpan.FromMinutes(5), cancellationToken);

        return Result<GetFastestLapResponse>.Success(ToResponse(fastestLap));
    }

    private static GetFastestLapResponse ToResponse(FastestLapItem item)
    {
        return new GetFastestLapResponse(item.RaceId, item.DriverId, item.DriverName, item.LapNumber, item.TimeMilliseconds);
    }
}

using Karting.Application.Abstractions.Caching;
using Karting.Application.Abstractions.Queries;
using Karting.Application.Common;
using Karting.Application.Features.Races.Common;
using Karting.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Karting.Application.Features.Races.GetResults;

public sealed class GetRaceResultsHandler(
    IRaceResultsQueryPort raceResultsQueryPort,
    IRaceCacheService raceCacheService,
    GetRaceResultsValidator validator,
    ILogger<GetRaceResultsHandler> logger)
{
    public async Task<Result<GetRaceResultsResponse>> HandleAsync(GetRaceResultsRequest request, CancellationToken cancellationToken)
    {
        var validation = validator.Validate(request);
        if (!validation.IsSuccess)
        {
            return Result<GetRaceResultsResponse>.Failure(validation.ErrorCode!, validation.ErrorMessage!);
        }

        var cacheKey = RaceCacheKeys.Results(request.RaceId);
        var cached = await raceCacheService.GetAsync<IReadOnlyList<RaceResultItem>>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            logger.LogInformation("Race results cache hit {RaceId}", request.RaceId);
            return Result<GetRaceResultsResponse>.Success(new GetRaceResultsResponse(cached));
        }

        var raceId = RaceId.From(request.RaceId);
        var results = await raceResultsQueryPort.GetRaceResultsAsync(raceId, cancellationToken);

        await raceCacheService.SetAsync(cacheKey, results, TimeSpan.FromMinutes(5), cancellationToken);

        return Result<GetRaceResultsResponse>.Success(new GetRaceResultsResponse(results));
    }
}

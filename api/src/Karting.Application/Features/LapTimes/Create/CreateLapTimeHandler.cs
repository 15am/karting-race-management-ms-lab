using Karting.Application.Abstractions.Caching;
using Karting.Application.Abstractions.Persistence;
using Karting.Application.Common;
using Karting.Application.Features.Races.Common;
using Karting.Domain.Entities;
using Karting.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Karting.Application.Features.LapTimes.Create;

public sealed class CreateLapTimeHandler(
    ILapTimeRepository lapTimeRepository,
    IUnitOfWork unitOfWork,
    IRaceCacheService raceCacheService,
    CreateLapTimeValidator validator,
    ILogger<CreateLapTimeHandler> logger)
{
    public async Task<Result<CreateLapTimeResponse>> HandleAsync(CreateLapTimeRequest request, CancellationToken cancellationToken)
    {
        var validation = validator.Validate(request);
        if (!validation.IsSuccess)
        {
            return Result<CreateLapTimeResponse>.Failure(validation.ErrorCode!, validation.ErrorMessage!);
        }

        LapTime lapTime;
        try
        {
            lapTime = LapTime.Create(request.RaceId, request.DriverId, request.LapNumber, request.TimeMilliseconds);
        }
        catch (DomainRuleException exception)
        {
            return Result<CreateLapTimeResponse>.Failure(ErrorCodes.Validation, exception.Message);
        }

        await lapTimeRepository.AddAsync(lapTime, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await raceCacheService.RemoveAsync(RaceCacheKeys.Results(request.RaceId), cancellationToken);
        await raceCacheService.RemoveAsync(RaceCacheKeys.FastestLap(request.RaceId), cancellationToken);

        logger.LogInformation("Lap added {LapId}", lapTime.Id.Value);
        logger.LogInformation("Cache invalidated for race {RaceId}", request.RaceId);

        var response = new CreateLapTimeResponse(
            lapTime.Id.Value,
            lapTime.RaceId.Value,
            lapTime.DriverId.Value,
            lapTime.LapNumber.Value,
            lapTime.TimeMilliseconds.Value,
            lapTime.CreatedAtUtc);

        return Result<CreateLapTimeResponse>.Success(response);
    }
}

using Karting.Application.Common;

namespace Karting.Application.Features.LapTimes.Create;

public sealed class CreateLapTimeValidator
{
    public Result Validate(CreateLapTimeRequest request)
    {
        if (request.RaceId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "RaceId is required.");
        }

        if (request.DriverId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "DriverId is required.");
        }

        if (request.LapNumber <= 0)
        {
            return Result.Failure(ErrorCodes.Validation, "Lap number must be greater than zero.");
        }

        if (request.TimeMilliseconds <= 0)
        {
            return Result.Failure(ErrorCodes.Validation, "TimeMilliseconds must be greater than zero.");
        }

        return Result.Success();
    }
}

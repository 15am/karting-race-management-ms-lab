using Karting.Application.Common;

namespace Karting.Application.Features.LapTimes.Create;

/// <summary>
/// Represents the CreateLapTimeValidator class.
/// </summary>
public sealed class CreateLapTimeValidator
{
    /// <summary>
    /// Executes Validate.
    /// </summary>
    /// <param name="request">The request value.</param>
    /// <returns>The operation result.</returns>
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

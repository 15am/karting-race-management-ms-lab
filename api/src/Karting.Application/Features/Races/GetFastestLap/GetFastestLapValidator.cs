using Karting.Application.Common;

namespace Karting.Application.Features.Races.GetFastestLap;

/// <summary>
/// Represents the GetFastestLapValidator class.
/// </summary>
public sealed class GetFastestLapValidator
{
    /// <summary>
    /// Executes Validate.
    /// </summary>
    /// <param name="request">The request value.</param>
    /// <returns>The operation result.</returns>
    public Result Validate(GetFastestLapRequest request)
    {
        if (request.RaceId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "RaceId is required.");
        }

        return Result.Success();
    }
}

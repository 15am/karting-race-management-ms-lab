using Karting.Application.Common;

namespace Karting.Application.Features.Races.GetResults;

/// <summary>
/// Represents the GetRaceResultsValidator class.
/// </summary>
public sealed class GetRaceResultsValidator
{
    /// <summary>
    /// Executes Validate.
    /// </summary>
    /// <param name="request">The request value.</param>
    /// <returns>The operation result.</returns>
    public Result Validate(GetRaceResultsRequest request)
    {
        if (request.RaceId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "RaceId is required.");
        }

        return Result.Success();
    }
}

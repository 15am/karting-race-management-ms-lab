using Karting.Application.Common;

namespace Karting.Application.Features.Races.GetResults;

public sealed class GetRaceResultsValidator
{
    public Result Validate(GetRaceResultsRequest request)
    {
        if (request.RaceId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "RaceId is required.");
        }

        return Result.Success();
    }
}

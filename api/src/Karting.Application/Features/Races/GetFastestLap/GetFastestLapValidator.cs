using Karting.Application.Common;

namespace Karting.Application.Features.Races.GetFastestLap;

public sealed class GetFastestLapValidator
{
    public Result Validate(GetFastestLapRequest request)
    {
        if (request.RaceId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "RaceId is required.");
        }

        return Result.Success();
    }
}

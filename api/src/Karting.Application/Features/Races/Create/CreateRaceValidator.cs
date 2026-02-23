using Karting.Application.Common;

namespace Karting.Application.Features.Races.Create;

public sealed class CreateRaceValidator
{
    public Result Validate(CreateRaceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Result.Failure(ErrorCodes.Validation, "Race name is required.");
        }

        if (request.DateUtc.Kind != DateTimeKind.Utc)
        {
            return Result.Failure(ErrorCodes.Validation, "Race date must be UTC.");
        }

        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return Result.Failure(ErrorCodes.Validation, "Race status is required.");
        }

        return Result.Success();
    }
}

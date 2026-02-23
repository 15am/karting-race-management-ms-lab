using Karting.Application.Common;

namespace Karting.Application.Features.Races.Create;

/// <summary>
/// Represents the CreateRaceValidator class.
/// </summary>
public sealed class CreateRaceValidator
{
    /// <summary>
    /// Executes Validate.
    /// </summary>
    /// <param name="request">The request value.</param>
    /// <returns>The operation result.</returns>
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

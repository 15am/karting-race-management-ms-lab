using Karting.Application.Common;

namespace Karting.Application.Features.Drivers.Create;

/// <summary>
/// Represents the CreateDriverValidator class.
/// </summary>
public sealed class CreateDriverValidator
{
    /// <summary>
    /// Executes Validate.
    /// </summary>
    /// <param name="request">The request value.</param>
    /// <returns>The operation result.</returns>
    public Result Validate(CreateDriverRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Result.Failure(ErrorCodes.Validation, "Driver name is required.");
        }

        if (request.Name.Trim().Length > 80)
        {
            return Result.Failure(ErrorCodes.Validation, "Driver name cannot exceed 80 characters.");
        }

        return Result.Success();
    }
}

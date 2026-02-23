using Karting.Application.Common;

namespace Karting.Application.Features.Drivers.Create;

public sealed class CreateDriverValidator
{
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

using Karting.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Karting.Api.Extensions;

public static class ControllerProblemExtensions
{
    public static IActionResult ToProblem(this ControllerBase controller, string? errorCode, string? errorMessage)
    {
        var detail = errorMessage ?? "Request failed.";
        if (errorCode == ErrorCodes.Validation)
        {
            return controller.BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error",
                Detail = detail,
            });
        }

        if (errorCode == ErrorCodes.NotFound)
        {
            return controller.NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = detail,
            });
        }

        if (errorCode == ErrorCodes.Conflict)
        {
            return controller.Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = detail,
            });
        }

        return controller.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: detail, title: "Unexpected error");
    }
}

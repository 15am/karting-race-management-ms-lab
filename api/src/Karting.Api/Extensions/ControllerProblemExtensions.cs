using Karting.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Karting.Api.Extensions;

/// <summary>
/// Represents the ControllerProblemExtensions class.
/// </summary>
public static class ControllerProblemExtensions
{
    /// <summary>
    /// Executes ToProblem.
    /// </summary>
    /// <param name="controller">The controller value.</param>
    /// <param name="errorCode">The errorCode value.</param>
    /// <param name="errorMessage">The errorMessage value.</param>
    /// <returns>The operation result.</returns>
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

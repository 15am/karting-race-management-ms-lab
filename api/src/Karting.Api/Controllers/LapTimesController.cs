using Karting.Api.Extensions;
using Karting.Application.Features.LapTimes.Create;
using Microsoft.AspNetCore.Mvc;

namespace Karting.Api.Controllers;

/// <summary>
/// Represents the lap-times API controller.
/// </summary>
/// <param name="handler">The create-lap-time handler dependency.</param>
[ApiController]
[Route("laptimes")]
public sealed class LapTimesController(CreateLapTimeHandler handler) : ControllerBase
{
    /// <summary>
    /// Creates a new lap-time entry.
    /// </summary>
    /// <param name="request">The lap-time creation request payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HTTP action result.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateLapTimeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateLapTimeRequest request, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken);
        if (!result.IsSuccess)
        {
            return this.ToProblem(result.ErrorCode, result.ErrorMessage);
        }

        return Created($"/laptimes/{result.Value!.Id}", result.Value);
    }
}

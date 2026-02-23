using Karting.Api.Extensions;
using Karting.Application.Features.LapTimes.Create;
using Microsoft.AspNetCore.Mvc;

namespace Karting.Api.Controllers;

[ApiController]
[Route("laptimes")]
public sealed class LapTimesController(CreateLapTimeHandler handler) : ControllerBase
{
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

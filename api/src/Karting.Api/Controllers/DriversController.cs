using Karting.Api.Extensions;
using Karting.Application.Features.Drivers.Create;
using Microsoft.AspNetCore.Mvc;

namespace Karting.Api.Controllers;

/// <summary>
/// Represents the drivers API controller.
/// </summary>
/// <param name="handler">The create-driver handler dependency.</param>
[ApiController]
[Route("drivers")]
public sealed class DriversController(CreateDriverHandler handler) : ControllerBase
{
    /// <summary>
    /// Creates a new driver.
    /// </summary>
    /// <param name="request">The driver creation request payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HTTP action result.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateDriverResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDriverRequest request, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken);
        if (!result.IsSuccess)
        {
            return this.ToProblem(result.ErrorCode, result.ErrorMessage);
        }

        return Created($"/drivers/{result.Value!.Id}", result.Value);
    }
}

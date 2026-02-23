using Karting.Api.Extensions;
using Karting.Application.Features.Races.Create;
using Karting.Application.Features.Races.GetFastestLap;
using Karting.Application.Features.Races.GetResults;
using Microsoft.AspNetCore.Mvc;

namespace Karting.Api.Controllers;

/// <summary>
/// Represents the races API controller.
/// </summary>
/// <param name="createRaceHandler">The create-race handler dependency.</param>
/// <param name="getRaceResultsHandler">The race-results handler dependency.</param>
/// <param name="getFastestLapHandler">The fastest-lap handler dependency.</param>
[ApiController]
[Route("races")]
public sealed class RacesController(
    CreateRaceHandler createRaceHandler,
    GetRaceResultsHandler getRaceResultsHandler,
    GetFastestLapHandler getFastestLapHandler) : ControllerBase
{
    /// <summary>
    /// Creates a race.
    /// </summary>
    /// <param name="request">The race creation request payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HTTP action result.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateRaceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRaceRequest request, CancellationToken cancellationToken)
    {
        var result = await createRaceHandler.HandleAsync(request, cancellationToken);
        if (!result.IsSuccess)
        {
            return this.ToProblem(result.ErrorCode, result.ErrorMessage);
        }

        return Created($"/races/{result.Value!.Id}", result.Value);
    }

    /// <summary>
    /// Gets race aggregate results ordered by total time.
    /// </summary>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HTTP action result.</returns>
    [HttpGet("{raceId:guid}/results")]
    [ProducesResponseType(typeof(GetRaceResultsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetResults(Guid raceId, CancellationToken cancellationToken)
    {
        var result = await getRaceResultsHandler.HandleAsync(new GetRaceResultsRequest(raceId), cancellationToken);
        if (!result.IsSuccess)
        {
            return this.ToProblem(result.ErrorCode, result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the fastest lap for the specified race.
    /// </summary>
    /// <param name="raceId">The race identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HTTP action result.</returns>
    [HttpGet("{raceId:guid}/fastest-lap")]
    [ProducesResponseType(typeof(GetFastestLapResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFastestLap(Guid raceId, CancellationToken cancellationToken)
    {
        var result = await getFastestLapHandler.HandleAsync(new GetFastestLapRequest(raceId), cancellationToken);
        if (!result.IsSuccess)
        {
            return this.ToProblem(result.ErrorCode, result.ErrorMessage);
        }

        return Ok(result.Value);
    }
}

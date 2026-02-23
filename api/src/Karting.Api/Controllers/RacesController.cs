using Karting.Api.Extensions;
using Karting.Application.Features.Races.Create;
using Karting.Application.Features.Races.GetFastestLap;
using Karting.Application.Features.Races.GetResults;
using Microsoft.AspNetCore.Mvc;

namespace Karting.Api.Controllers;

[ApiController]
[Route("races")]
public sealed class RacesController(
    CreateRaceHandler createRaceHandler,
    GetRaceResultsHandler getRaceResultsHandler,
    GetFastestLapHandler getFastestLapHandler) : ControllerBase
{
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

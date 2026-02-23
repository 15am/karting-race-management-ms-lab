using Karting.Application.Abstractions.Persistence;
using Karting.Application.Common;
using Karting.Domain.Entities;
using Karting.Domain.Enums;
using Karting.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Karting.Application.Features.Races.Create;

/// <summary>
/// Represents the CreateRaceHandler class.
/// </summary>
public sealed class CreateRaceHandler(
    IRaceRepository raceRepository,
    IUnitOfWork unitOfWork,
    CreateRaceValidator validator,
    ILogger<CreateRaceHandler> logger)
{
    /// <summary>
    /// Executes HandleAsync.
    /// </summary>
    /// <param name="request">The request value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public async Task<Result<CreateRaceResponse>> HandleAsync(CreateRaceRequest request, CancellationToken cancellationToken)
    {
        var validation = validator.Validate(request);
        if (!validation.IsSuccess)
        {
            return Result<CreateRaceResponse>.Failure(validation.ErrorCode!, validation.ErrorMessage!);
        }

        if (!Enum.TryParse<RaceStatus>(request.Status, true, out var status))
        {
            return Result<CreateRaceResponse>.Failure(ErrorCodes.Validation, "Race status is invalid.");
        }

        Race race;
        try
        {
            race = Race.Create(request.Name, request.DateUtc, status);
        }
        catch (DomainRuleException exception)
        {
            return Result<CreateRaceResponse>.Failure(ErrorCodes.Validation, exception.Message);
        }

        await raceRepository.AddAsync(race, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Race created {RaceId}", race.Id.Value);

        var response = new CreateRaceResponse(race.Id.Value, race.Name.Value, race.DateUtc, race.Status.ToString());
        return Result<CreateRaceResponse>.Success(response);
    }
}

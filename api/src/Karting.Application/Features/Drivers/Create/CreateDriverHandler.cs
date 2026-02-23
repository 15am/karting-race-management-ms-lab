using Karting.Application.Abstractions.Persistence;
using Karting.Application.Common;
using Karting.Domain.Entities;
using Karting.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Karting.Application.Features.Drivers.Create;

public sealed class CreateDriverHandler(
    IDriverRepository driverRepository,
    IUnitOfWork unitOfWork,
    CreateDriverValidator validator,
    ILogger<CreateDriverHandler> logger)
{
    public async Task<Result<CreateDriverResponse>> HandleAsync(CreateDriverRequest request, CancellationToken cancellationToken)
    {
        var validation = validator.Validate(request);
        if (!validation.IsSuccess)
        {
            return Result<CreateDriverResponse>.Failure(validation.ErrorCode!, validation.ErrorMessage!);
        }

        Driver driver;
        try
        {
            driver = Driver.Create(request.Name);
        }
        catch (DomainRuleException exception)
        {
            return Result<CreateDriverResponse>.Failure(ErrorCodes.Validation, exception.Message);
        }

        await driverRepository.AddAsync(driver, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Driver created {DriverId}", driver.Id.Value);

        var response = new CreateDriverResponse(driver.Id.Value, driver.Name.Value, driver.CreatedAtUtc);
        return Result<CreateDriverResponse>.Success(response);
    }
}

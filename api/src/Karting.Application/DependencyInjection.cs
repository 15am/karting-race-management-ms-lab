using Karting.Application.Features.Drivers.Create;
using Karting.Application.Features.LapTimes.Create;
using Karting.Application.Features.Races.Create;
using Karting.Application.Features.Races.GetFastestLap;
using Karting.Application.Features.Races.GetResults;
using Microsoft.Extensions.DependencyInjection;

namespace Karting.Application;

/// <summary>
/// Represents the DependencyInjection class.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Executes AddApplication.
    /// </summary>
    /// <param name="services">The services value.</param>
    /// <returns>The operation result.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateDriverValidator>();
        services.AddScoped<CreateRaceValidator>();
        services.AddScoped<CreateLapTimeValidator>();
        services.AddScoped<GetRaceResultsValidator>();
        services.AddScoped<GetFastestLapValidator>();

        services.AddScoped<CreateDriverHandler>();
        services.AddScoped<CreateRaceHandler>();
        services.AddScoped<CreateLapTimeHandler>();
        services.AddScoped<GetRaceResultsHandler>();
        services.AddScoped<GetFastestLapHandler>();

        return services;
    }
}

using Karting.Application.Abstractions.Caching;
using Karting.Application.Abstractions.Persistence;
using Karting.Application.Abstractions.Queries;
using Karting.Infrastructure.Caching;
using Karting.Infrastructure.Persistence;
using Karting.Infrastructure.Persistence.Queries;
using Karting.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Karting.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["Database:Provider"] ?? "InMemory";

        if (string.Equals(provider, "SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            var connectionString = configuration.GetConnectionString("SqlServer")
                ?? throw new InvalidOperationException("ConnectionStrings:SqlServer is required when using SqlServer provider.");

            services.AddDbContext<KartingDbContext>(options => options.UseSqlServer(connectionString));
            return RegisterServices(services);
        }

        services.AddDbContext<KartingDbContext>(options => options.UseInMemoryDatabase("karting-race-management"));
        return RegisterServices(services);
    }

    private static IServiceCollection RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IRaceRepository, RaceRepository>();
        services.AddScoped<ILapTimeRepository, LapTimeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IRaceResultsQueryPort, SqlServerRaceResultsQueryPort>();
        services.AddScoped<IFastestLapQueryPort, SqlServerFastestLapQueryPort>();
        services.AddScoped<IRaceCacheService, DistributedRaceCacheService>();

        return services;
    }
}

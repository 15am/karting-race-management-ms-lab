# Provider Extension Notes

## Current behavior
- `Karting.Infrastructure.DependencyInjection.AddInfrastructure` reads `Database:Provider`.
- `SqlServer` configures `UseSqlServer` with `ConnectionStrings:SqlServer`.
- Any other value configures `UseInMemoryDatabase` for local/test execution.

## How to add another SQL provider
1. Create new query adapters implementing:
- `IRaceResultsQueryPort`
- `IFastestLapQueryPort`
2. Create provider-specific write persistence wiring in `AddInfrastructure`.
3. Keep application handlers unchanged and switch only DI registration.
4. Preserve cache keys and invalidation contract:
- `race:{raceId}:results`
- `race:{raceId}:fastestlap`

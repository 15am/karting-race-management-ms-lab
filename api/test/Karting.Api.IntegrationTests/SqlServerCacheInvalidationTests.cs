using System.Net.Http.Json;
using System.Text.Json;

namespace Karting.Api.IntegrationTests;

public sealed class SqlServerCacheInvalidationTests
{
    [SqlServerFact]
    public async Task LapCreation_AfterCachedReads_RefreshesAggregateResponses()
    {
        var connectionString = SqlServerIntegrationSettings.GetConnectionString();
        await SqlServerScriptRunner.ApplySqlAssetsAsync(connectionString, CancellationToken.None);
        await SqlServerScriptRunner.ResetRaceDataAsync(connectionString, CancellationToken.None);

        await using var factory = new SqlServerKartingWebApplicationFactory(connectionString);
        using var client = factory.CreateClient();

        var firstDriverId = await CreateDriverAsync(client, "Cache-A");
        var secondDriverId = await CreateDriverAsync(client, "Cache-B");
        var raceId = await CreateRaceAsync(client, "Cache-Race");

        await CreateLapAsync(client, raceId, firstDriverId, 1, 50000);

        var resultsBefore = await client.GetFromJsonAsync<JsonElement>($"/races/{raceId}/results");
        var fastestBefore = await client.GetFromJsonAsync<JsonElement>($"/races/{raceId}/fastest-lap");
        Assert.Equal(firstDriverId, resultsBefore.GetProperty("results").EnumerateArray().First().GetProperty("driverId").GetGuid());
        Assert.Equal(firstDriverId, fastestBefore.GetProperty("driverId").GetGuid());

        await CreateLapAsync(client, raceId, secondDriverId, 1, 49000);

        var resultsAfter = await client.GetFromJsonAsync<JsonElement>($"/races/{raceId}/results");
        var fastestAfter = await client.GetFromJsonAsync<JsonElement>($"/races/{raceId}/fastest-lap");
        Assert.Equal(secondDriverId, resultsAfter.GetProperty("results").EnumerateArray().First().GetProperty("driverId").GetGuid());
        Assert.Equal(secondDriverId, fastestAfter.GetProperty("driverId").GetGuid());
        Assert.Equal(49000, fastestAfter.GetProperty("timeMilliseconds").GetInt32());
    }

    private static async Task<Guid> CreateDriverAsync(HttpClient client, string name)
    {
        var response = await client.PostAsJsonAsync("/drivers", new { name });
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        return payload.GetProperty("id").GetGuid();
    }

    private static async Task<Guid> CreateRaceAsync(HttpClient client, string name)
    {
        var response = await client.PostAsJsonAsync("/races", new { name, dateUtc = DateTime.UtcNow, status = "Scheduled" });
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        return payload.GetProperty("id").GetGuid();
    }

    private static async Task CreateLapAsync(HttpClient client, Guid raceId, Guid driverId, int lapNumber, int timeMilliseconds)
    {
        var response = await client.PostAsJsonAsync("/laptimes", new { raceId, driverId, lapNumber, timeMilliseconds });
        response.EnsureSuccessStatusCode();
    }
}

using System.Net.Http.Json;
using System.Text.Json;

namespace Karting.Api.IntegrationTests;

public sealed class SqlServerAggregateFlowTests
{
    [SqlServerFact]
    public async Task AggregateEndpoints_SqlServerProvider_ReturnDeterministicData()
    {
        var connectionString = SqlServerIntegrationSettings.GetConnectionString();
        await SqlServerScriptRunner.ApplySqlAssetsAsync(connectionString, CancellationToken.None);
        await SqlServerScriptRunner.ResetRaceDataAsync(connectionString, CancellationToken.None);

        await using var factory = new SqlServerKartingWebApplicationFactory(connectionString);
        using var client = factory.CreateClient();

        var firstDriverId = await CreateDriverAsync(client, "Driver-A");
        var secondDriverId = await CreateDriverAsync(client, "Driver-B");
        var raceId = await CreateRaceAsync(client, "SQL-Race");

        await CreateLapAsync(client, raceId, firstDriverId, 1, 50000);
        await CreateLapAsync(client, raceId, firstDriverId, 2, 51000);
        await CreateLapAsync(client, raceId, secondDriverId, 1, 52000);
        await CreateLapAsync(client, raceId, secondDriverId, 2, 53000);

        var resultsPayload = await client.GetFromJsonAsync<JsonElement>($"/races/{raceId}/results");
        var resultItems = resultsPayload.GetProperty("results").EnumerateArray().ToArray();
        Assert.Equal(2, resultItems.Length);
        Assert.Equal(firstDriverId, resultItems[0].GetProperty("driverId").GetGuid());
        Assert.Equal(101000, resultItems[0].GetProperty("totalTimeMilliseconds").GetInt32());
        Assert.Equal(secondDriverId, resultItems[1].GetProperty("driverId").GetGuid());

        var fastestPayload = await client.GetFromJsonAsync<JsonElement>($"/races/{raceId}/fastest-lap");
        Assert.Equal(firstDriverId, fastestPayload.GetProperty("driverId").GetGuid());
        Assert.Equal(1, fastestPayload.GetProperty("lapNumber").GetInt32());
        Assert.Equal(50000, fastestPayload.GetProperty("timeMilliseconds").GetInt32());
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

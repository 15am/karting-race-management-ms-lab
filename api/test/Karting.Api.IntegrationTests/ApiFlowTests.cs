using System.Net;
using System.Net.Http.Json;

namespace Karting.Api.IntegrationTests;

public sealed class ApiFlowTests(KartingWebApplicationFactory factory) : IClassFixture<KartingWebApplicationFactory>
{
    [Fact]
    public async Task Endpoints_CreateAndAggregateFlow_ReturnsExpectedPayloads()
    {
        using var client = factory.CreateClient();

        var driverCreate = await client.PostAsJsonAsync("/drivers", new { name = "Ayrton" });
        driverCreate.EnsureSuccessStatusCode();
        var driverJson = await driverCreate.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        var driverId = Guid.Parse(driverJson!["id"].ToString()!);

        var raceCreate = await client.PostAsJsonAsync("/races", new
        {
            name = "Sprint",
            dateUtc = DateTime.UtcNow,
            status = "Scheduled",
        });
        raceCreate.EnsureSuccessStatusCode();
        var raceJson = await raceCreate.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        var raceId = Guid.Parse(raceJson!["id"].ToString()!);

        var lapCreate = await client.PostAsJsonAsync("/laptimes", new
        {
            raceId,
            driverId,
            lapNumber = 1,
            timeMilliseconds = 50234,
        });
        lapCreate.EnsureSuccessStatusCode();

        var resultsResponse = await client.GetAsync($"/races/{raceId}/results");
        resultsResponse.EnsureSuccessStatusCode();
        var resultsPayload = await resultsResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        var fastestResponse = await client.GetAsync($"/races/{raceId}/fastest-lap");
        fastestResponse.EnsureSuccessStatusCode();
        var fastestPayload = await fastestResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        Assert.NotNull(resultsPayload);
        Assert.NotNull(fastestPayload);
    }

    [Fact]
    public async Task Endpoints_InvalidDriverRequest_ReturnsProblemDetails()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/drivers", new { name = "" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}

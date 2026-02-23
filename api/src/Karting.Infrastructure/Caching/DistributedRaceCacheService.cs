using System.Text;
using System.Text.Json;
using Karting.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Karting.Infrastructure.Caching;

public sealed class DistributedRaceCacheService(IDistributedCache distributedCache) : IRaceCacheService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var bytes = await distributedCache.GetAsync(key, cancellationToken);
        if (bytes is null)
        {
            return default;
        }

        var json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(json, SerializerOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl,
        };

        var json = JsonSerializer.Serialize(value, SerializerOptions);
        var bytes = Encoding.UTF8.GetBytes(json);

        await distributedCache.SetAsync(key, bytes, options, cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return distributedCache.RemoveAsync(key, cancellationToken);
    }
}

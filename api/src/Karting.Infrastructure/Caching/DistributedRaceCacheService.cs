using System.Text;
using System.Text.Json;
using Karting.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Karting.Infrastructure.Caching;

/// <summary>
/// Represents the DistributedRaceCacheService class.
/// </summary>
public sealed class DistributedRaceCacheService(IDistributedCache distributedCache) : IRaceCacheService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Represents a publicly exposed member.
    /// </summary>
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

    /// <summary>
    /// Represents a publicly exposed member.
    /// </summary>
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

    /// <summary>
    /// Executes RemoveAsync.
    /// </summary>
    /// <param name="key">The key value.</param>
    /// <param name="cancellationToken">The cancellationToken value.</param>
    /// <returns>The operation result.</returns>
    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return distributedCache.RemoveAsync(key, cancellationToken);
    }
}

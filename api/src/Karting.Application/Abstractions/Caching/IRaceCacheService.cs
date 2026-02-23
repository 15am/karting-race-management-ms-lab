namespace Karting.Application.Abstractions.Caching;

/// <summary>
/// Represents the IRaceCacheService interface.
/// </summary>
public interface IRaceCacheService
{
    /// <summary>
    /// Gets a cached value by key.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="T">The cached value type.</typeparam>
    /// <returns>The cached value when found; otherwise null.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);

    /// <summary>
    /// Stores a value in cache with a time-to-live value.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="ttl">The cache time-to-live.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="T">The cached value type.</typeparam>
    /// <returns>A task that completes when the cache write finishes.</returns>
    Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a value from cache by key.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that completes when cache removal finishes.</returns>
    Task RemoveAsync(string key, CancellationToken cancellationToken);
}

using Tajan.Standard.Domain.Abstratcions;

namespace Tajan.Standard.Application.Abstractions;

public interface ICacheProvider
{
    Task<CachedValue<T>> GetAsync<T>(CacheKey key, CancellationToken ct = default);

    Task<List<CachedValue<T>>> GetAsync<T>(List<CacheKey> keyList, CancellationToken ct = default);

    Task<CachedValue<T>> GetAsync<T>(CacheKey key, Func<ValueTask<T>> setter, TimeSpan expiration, CancellationToken ct = default);

    Task<CachedValue<T>> GetAsync<T>(CacheKey key, Func<ValueTask<T>> setter, CancellationToken ct = default);

    Task SetAsync<T>(CacheKey key, T value, CancellationToken ct = default);

    Task SetAsync<T>(CacheKey key, T value, TimeSpan expiration, CancellationToken ct = default);

    Task RemoveAsync(CacheKey key, CancellationToken ct = default);

    Task RemoveAsync(params CacheKey[] keys);

}

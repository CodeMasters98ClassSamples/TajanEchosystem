using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using StackExchange.Redis;
using Tajan.Standard.Application.Abstractions;
using Tajan.Standard.Application.Extensions;
using Tajan.Standard.Domain.Abstratcions;
using Tajan.Standard.Infrastructure.CacheProvider.ConnectionFactory;

namespace Tajan.Standard.Infrastructure.CacheProvider;

public class RedisCacheProvider(
    [FromKeyedServices(nameof(RedisCacheProvider))] ResiliencePipeline resiliencePipeline,
    IRedisConnectionFactory connectionFactory,
    ILogger<RedisCacheProvider> logger)
    : ICacheProvider
{

    public async Task<CachedValue<T>> GetAsync<T>(CacheKey key, CancellationToken ct = default)
        => await UseReplica<T>(database
            => database.StringGetAsync(key.Value), ct);

    public async Task<List<CachedValue<T>>> GetAsync<T>(List<CacheKey> keyList, CancellationToken ct = default)
        => await UseReplicaList<T>(database
            => database.StringGetAsync(keyList.Select(x => new RedisKey(x.Value)).ToArray()), ct);

    public async Task SetAsync<T>(CacheKey key, T value, CancellationToken ct = default)
        => await UseMaster(database
            => database.StringSetAsync(key.Value, value.Serialize()), ct);

    public async Task SetAsync<T>(CacheKey key, T value, TimeSpan expiration, CancellationToken ct = default)
        => await UseMaster(database
            => database.StringSetAsync(key.Value, value.Serialize(), expiration), ct);

    public async Task RemoveAsync(CacheKey key, CancellationToken ct = default)
        => await UseMaster(database
            => database.KeyDeleteAsync(key.Value), ct);

    public async Task<CachedValue<T>> GetAsync<T>(CacheKey key, Func<ValueTask<T>> setter, TimeSpan expiration, CancellationToken ct = default)
    {
        CachedValue<T> cachedValue = await GetAsync<T>(key, ct);
        if (cachedValue.IsValid)
            return cachedValue;

        T newValue = await setter();
        await SetAsync(key, newValue, expiration, ct);
        return CachedValue.Valid(newValue);
    }

    public async Task<CachedValue<T>> GetAsync<T>(CacheKey key, Func<ValueTask<T>> setter, CancellationToken ct = default)
    {
        CachedValue<T> cachedValue = await GetAsync<T>(key, ct);
        if (cachedValue.IsValid)
            return cachedValue;

        T newValue = await setter();
        await SetAsync(key, newValue, ct);
        return CachedValue.Valid(newValue);
    }

    public async Task RemoveAsync(params CacheKey[] keys)
        => await UseMaster(async database =>
        {
            foreach (CacheKey key in keys)
                await database.KeyDeleteAsync(key.Value);
        });

    private async Task UseMaster(Func<IDatabase, Task> function, CancellationToken ct = default)
    {
        logger.LogInformation("Trying to Write On Redis Master Node");
        Outcome<bool> outcome = await resiliencePipeline.ExecuteOutcomeAsync(async (ctx, state) =>
        {
            IDatabase database = await connectionFactory.GetMasterDatabase();
            await function(database);
            return Outcome.FromResult(true);
        }, ResilienceContextPool.Shared.Get(ct), "state");

        if (outcome.Exception is BrokenCircuitException)
            logger.LogInformation("Write Action on Redis Master Node faild because of Broken Circuit");
    }


    private async Task<CachedValue<T>> UseReplica<T>(Func<IDatabase, Task<RedisValue>> function, CancellationToken ct = default)
    {
        logger.LogInformation("Trying to Read From Redis Replica Nodes");
        Outcome<RedisValue> outcome = await resiliencePipeline.ExecuteOutcomeAsync(async (ctx, state) =>
        {
            IDatabase database = await connectionFactory.GetReplicaDatabase();
            return Outcome.FromResult(await function(database));

        }, ResilienceContextPool.Shared.Get(ct), "state");

        if (outcome.Exception is BrokenCircuitException)
        {
            logger.LogInformation("Read Action on Redis Replica Nodes faild because of Broken Circuit");
            return CachedValue.Invalid<T>();
        }

        string cachedValue = outcome.Result.ToString();
        return cachedValue.DoesNotHaveValue()
           ? CachedValue.Invalid<T>()
           : CachedValue.Valid(cachedValue.Deserialize<T>());
    }

    private async Task<List<CachedValue<T>>> UseReplicaList<T>(Func<IDatabase, Task<RedisValue[]>> function, CancellationToken ct = default)
    {
        logger.LogInformation("Trying to Read From Redis Replica Nodes");

        Outcome<RedisValue[]> outcome = await resiliencePipeline.ExecuteOutcomeAsync(async (ctx, state) =>
        {
            IDatabase database = await connectionFactory.GetReplicaDatabase();
            return Outcome.FromResult(await function(database));

        }, ResilienceContextPool.Shared.Get(ct), "state");

        if (outcome.Exception is BrokenCircuitException)
        {
            logger.LogInformation("Read Action on Redis Replica Nodes faild because of Broken Circuit");
            return [CachedValue.Invalid<T>()];
        }

        List<CachedValue<T>> result = [];
        foreach (var item in outcome.Result)
        {
            var value = item.ToString().DoesNotHaveValue()
                   ? CachedValue.Invalid<T>()
                   : CachedValue.Valid(item.ToString().Deserialize<T>());

            result.Add(value);
        }

        return result;
    }
}

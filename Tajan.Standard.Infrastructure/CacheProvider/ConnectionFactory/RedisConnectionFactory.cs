using StackExchange.Redis;

namespace Tajan.Standard.Infrastructure.CacheProvider.ConnectionFactory;

internal class RedisConnectionFactory(
 ConfigurationOptions masterConfiguration,
 ConfigurationOptions replicaConfiguration)
 : IRedisConnectionFactory
{
    public async Task<IDatabase> GetMasterDatabase()
        => (await ConnectionMultiplexer.ConnectAsync(masterConfiguration)).GetDatabase();

    public async Task<IDatabase> GetReplicaDatabase()
        => (await ConnectionMultiplexer.ConnectAsync(replicaConfiguration)).GetDatabase();
}

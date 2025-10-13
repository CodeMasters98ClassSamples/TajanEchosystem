using StackExchange.Redis;

namespace Tajan.Standard.Infrastructure.CacheProvider.ConnectionFactory;

public interface IRedisConnectionFactory
{
    Task<IDatabase> GetMasterDatabase();

    Task<IDatabase> GetReplicaDatabase();
}

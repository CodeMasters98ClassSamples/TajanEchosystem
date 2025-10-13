using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Tajan.Standard.Domain.Abstratcions;
using Tajan.Standard.Domain.Settings;

namespace Tajan.Standard.Infrastructure.CacheProvider.HealthChecks;

public class ReadFromReplicaRedisHealthCheck(IOptionsMonitor<RedisSettings> options) : IHealthCheck
{
    private readonly RedisSettings _settings = options.CurrentValue;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        if (_settings.ReplicaNodes is null)
            return await Task.FromResult(new HealthCheckResult(HealthStatus.Healthy, "there is no replica node"));

        try
        {
            EndPointCollection replicaEndPoints = [];

            if (_settings.ReplicaNodes is not null)
                foreach (var host in _settings.ReplicaNodes.DistinctBy(h => h.ToString()))
                    replicaEndPoints.Add(host.Host, host.Port);

            ConfigurationOptions replicaConfiguration = new()
            {
                ServiceName = _settings.ServiceName,
                EndPoints = replicaEndPoints
            };

            IDatabase database = (await ConnectionMultiplexer.ConnectAsync(replicaConfiguration)).GetDatabase();

            CacheKey cacheKey = CacheKey.ConnectionTest();
            await database.StringGetAsync(cacheKey.Value);

            return await Task.FromResult(new HealthCheckResult(HealthStatus.Healthy));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy, ex.Message));
        }
    }
}

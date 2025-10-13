using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Tajan.Standard.Domain.Abstratcions;
using Tajan.Standard.Domain.Settings;

namespace Tajan.Standard.Infrastructure.CacheProvider.HealthChecks;

public class WriteOnMasterRedisHealthCheck(IOptionsMonitor<RedisSettings> options) : IHealthCheck
{
    private readonly RedisSettings _settings = options.CurrentValue;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            EndPointCollection masterEndPoints = [];
            masterEndPoints.Add(_settings.MasterNode.Host, _settings.MasterNode.Port);

            ConfigurationOptions masterConfiguration = new()
            {
                ServiceName = _settings.ServiceName,
                EndPoints = masterEndPoints
            };

            IDatabase database = (await ConnectionMultiplexer.ConnectAsync(masterConfiguration)).GetDatabase();

            CacheKey cacheKey = CacheKey.ConnectionTest();
            await database.StringSetAsync(cacheKey.Value, cacheKey.Value);

            return await Task.FromResult(new HealthCheckResult(HealthStatus.Healthy));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy, ex.Message));
        }
    }
}

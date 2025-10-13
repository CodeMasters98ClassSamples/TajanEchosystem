using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using StackExchange.Redis;
using Tajan.Standard.Application.Abstractions;
using Tajan.Standard.Domain.Settings;
using Tajan.Standard.Infrastructure.CacheProvider.ConnectionFactory;
using Tajan.Standard.Infrastructure.CacheProvider.HealthChecks;

namespace Tajan.Standard.Infrastructure.CacheProvider;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCacheProvider(this IServiceCollection services, IConfiguration configuration)
    {
        RedisSettings settings = configuration.GetSection("Redis").Get<RedisSettings>();
        services.Configure<RedisSettings>(configuration.GetSection("Redis"));
        ArgumentNullException.ThrowIfNull(settings.MasterNode, nameof(settings));

        EndPointCollection masterEndPoints = [];
        masterEndPoints.Add(settings.MasterNode.Host, settings.MasterNode.Port);

        ConfigurationOptions masterConfiguration = new()
        {
            ServiceName = settings.ServiceName,
            EndPoints = masterEndPoints
        };


        EndPointCollection replicaEndPoints = [];
        if (settings.ReplicaNodes is not null)
            foreach (Domain.Settings.HostInfo host in settings.GetAllDistinctNodes())
                replicaEndPoints.Add(host.Host, host.Port);

        ConfigurationOptions replicaConfiguration = new()
        {
            ServiceName = settings.ServiceName,
            EndPoints = replicaEndPoints,
            AbortOnConnectFail = false
        };

        if (settings.Resiliency is not null)
        {
            masterConfiguration.ConnectTimeout = settings.Resiliency.TimeoutInMilliseconds;
            masterConfiguration.ConnectRetry = settings.Resiliency.RetryCount;
            replicaConfiguration.ConnectRetry = settings.Resiliency.RetryCount;
            replicaConfiguration.ConnectTimeout = settings.Resiliency.TimeoutInMilliseconds;
        }


        services.AddSingleton<IRedisConnectionFactory>(new RedisConnectionFactory(masterConfiguration, replicaConfiguration));
        services.AddSingleton<ICacheProvider, RedisCacheProvider>();

        services.AddResiliencePipeline(nameof(RedisCacheProvider), builder =>
        {
            if (settings.Resiliency is null)
                return;

            builder
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    SamplingDuration = TimeSpan.FromSeconds(settings.Resiliency.SamplingDurationToCheckForFailureInSeconds),
                    MinimumThroughput = settings.Resiliency.MinimumThroughputToCheckForFailure,
                    FailureRatio = settings.Resiliency.FailureRatioToBreakTheCircuit,
                    BreakDuration = TimeSpan.FromSeconds(settings.Resiliency.BreakDurationInSeconds)
                });
        });

        services
            .AddHealthChecks()
            .AddRedis(settings.GetConnectionStrings(), tags: ["readiness"])
            .AddCheck<WriteOnMasterRedisHealthCheck>(nameof(WriteOnMasterRedisHealthCheck), tags: ["readiness"])
            .AddCheck<ReadFromReplicaRedisHealthCheck>(nameof(ReadFromReplicaRedisHealthCheck), tags: ["readiness"]);

        return services;
    }

}
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Tajan.OrderService.API.Settings;
using Tajan.OrderService.Application.Contracts.ExternalServices;

namespace Tajan.OrderService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly));

        //services.AddHttpClient<IProductService>((serviceProvider, client) =>
        //{
        //    //var settings = serviceProvider
        //    //    .GetRequiredService<IOptions<SampleSetting>>().Value;

        //    //client.DefaultRequestHeaders.Add("Authorization", settings.GitHubToken);
        //    //client.DefaultRequestHeaders.Add("User-Agent", settings.UserAgent);

        //    client.BaseAddress = new Uri(addresses.ProductServicePath);
        //})
        //.AddPolicyHandler(GetRetryPolicy())
        //.AddPolicyHandler(GetCircuitBreakerPolicy());


        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq();
        });

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                3,                  // retry count
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // 2s, 4s, 8s
            );
    }

    //circuit breaker policy
    static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                2,                  // break after 2 consecutive failures
                TimeSpan.FromSeconds(30) // stay open for 30s
            );
    }
}

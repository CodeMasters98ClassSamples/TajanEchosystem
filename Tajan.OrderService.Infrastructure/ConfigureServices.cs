using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tajan.OrderService.Infrastructure.Persistence.Contexts;
using Tajan.Standard.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using EFCoreSecondLevelCacheInterceptor;
using Tajan.OrderService.Domain.Services;
using Tajan.OrderService.Infrastructure.DomainServices;

namespace Tajan.OrderService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Tajan_Order_Db") ?? string.Empty;
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Empty connectionstring");

        bool useInMemoryDb = configuration.GetValue<bool>("UseInMemoryDataBase");

        // Register EF second-level cache services used by the DbContext interceptor
        services.AddMemoryCache();
        services.AddEFSecondLevelCache(options =>
            options.UseMemoryCacheProvider()
        );
        // Ensure the interceptor itself is available for AddInterceptors call
        services.AddSingleton<SecondLevelCacheInterceptor>();

        if (useInMemoryDb)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(option => option.UseInMemoryDatabase("AppDbContext"));
        }
        else
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((serviceProvider, options)
                   => options.UseSqlServer(connectionString)
                   .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));
        }

        services.AddHealthChecks()
                .AddSqlServer(connectionString);

    services.AddTransient<IOrderService, Tajan.OrderService.Infrastructure.DomainServices.OrderService>();
    // Basket repository
    services.AddScoped<Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories.IBasketRepository, Tajan.OrderService.Infrastructure.Persistence.Repositories.BasketRepository>();

        return services;
    }
}

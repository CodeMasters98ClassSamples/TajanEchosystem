using Microsoft.Extensions.DependencyInjection;

namespace Tajan.ProductService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly));

        return services;
    }
}


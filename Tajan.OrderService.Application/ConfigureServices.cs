using Microsoft.Extensions.DependencyInjection;

namespace Tajan.OrderService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly));
        return services;
    }
}

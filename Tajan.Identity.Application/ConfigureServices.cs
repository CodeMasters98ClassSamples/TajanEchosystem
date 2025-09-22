using Microsoft.Extensions.DependencyInjection;

namespace Tajan.Identity.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        var assembly = typeof(ConfigureServices).Assembly;

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        return services;
    }
}

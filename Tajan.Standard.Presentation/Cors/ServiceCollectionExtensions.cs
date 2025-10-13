using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tajan.Standard.Domain.Settings;

namespace Tajan.Standard.Presentation.Cors;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCorrelationContext(this IServiceCollection services)
    {
        services.TryAddScoped<CorrelationContext>();
        return services;
    }
}

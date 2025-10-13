using Microsoft.Extensions.DependencyInjection;

namespace Tajan.Standard.Presentation.GlobalExceptionHandler;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}

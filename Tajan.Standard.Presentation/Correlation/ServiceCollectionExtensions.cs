using Microsoft.Extensions.DependencyInjection;

namespace Tajan.Standard.Presentation.Correlation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBasicCors(this IServiceCollection services)
        => services.AddCors(options =>
        {
            options.AddDefaultPolicy(
            builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
        });
}

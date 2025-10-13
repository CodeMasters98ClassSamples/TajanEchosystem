using Microsoft.AspNetCore.Builder;

namespace Tajan.Standard.Presentation.Correlation;


public static class AppBuilderExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();
}

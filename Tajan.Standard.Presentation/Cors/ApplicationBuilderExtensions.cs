using Microsoft.AspNetCore.Builder;

namespace Tajan.Standard.Presentation.Cors;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseBasicCors(this IApplicationBuilder app)
        => app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

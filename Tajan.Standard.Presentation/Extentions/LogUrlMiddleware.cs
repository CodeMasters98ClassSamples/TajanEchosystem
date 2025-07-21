using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Tajan.Standard.Presentation.Extentions;

public class LogUrlMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogUrlMiddleware> _logger;

    public LogUrlMiddleware(RequestDelegate next, ILogger<LogUrlMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"Request URL: {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}");
        await this._next(context);
    }

}

public static class LogURLMiddlewareExtensions
{
    public static IApplicationBuilder UseLogUrl(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LogUrlMiddleware>();
    }
}

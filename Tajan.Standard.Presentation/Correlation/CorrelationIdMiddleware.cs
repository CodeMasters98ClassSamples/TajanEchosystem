using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using Tajan.Standard.Domain.Settings;

namespace Tajan.Standard.Presentation.Correlation;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context, CorrelationContext correlationContext)
    {
        Guid correlationId = GetCorrelationId(context);

        correlationContext.CorrelationId = correlationId;
        AddCorrelationIdHeaderToResponse(context, correlationId);

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
    }

    private static Guid GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out StringValues correlationIdStringValues))
        {
            if (Guid.TryParse(correlationIdStringValues.FirstOrDefault()?.ToString(), out Guid correlationId))
                return correlationId;
        }
        return Guid.NewGuid();
    }

    private static void AddCorrelationIdHeaderToResponse(HttpContext context, Guid correlationId)
        => context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append(CorrelationIdHeader, new[] { correlationId.ToString() });
            return Task.CompletedTask;
        });
}

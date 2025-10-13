using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace Tajan.Standard.Presentation.GlobalExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct = default)
    {
        logger.LogError(exception, "Exception occurred: message {Message} inner message {InnerMessage}", exception?.Message, exception?.InnerException?.ToString());

        if (!httpContext.Response.HasStarted)
        {
            await httpContext.Response.WriteAsync("", ct);
        }
        else
        {
            string message = exception?.Message;
            await httpContext.Response.WriteAsync(message, ct);
        }
        return true;
    }
}

using System.Security.Claims;

namespace Tajan.ApiGateway.Helpers
{
    public class UserIdHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        public UserIdHeaderMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var auth)
                && auth.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                && context.User?.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst("uid")?.Value
                          ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId) && !context.Request.Headers.ContainsKey("X-User-Id"))
                    context.Request.Headers.Append("X-User-Id", userId);
            }

            await _next(context);
        }
    }

    public static class UserIdHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserIdHeader(this IApplicationBuilder app)
            => app.UseMiddleware<UserIdHeaderMiddleware>();
    }

}

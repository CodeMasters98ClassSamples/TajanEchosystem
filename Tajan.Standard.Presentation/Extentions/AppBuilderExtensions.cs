using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Tajan.Standard.Presentation.Extentions;

public static class AppBuilderExtensions
{
    public static WebApplication MapHealthCheckEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).ShortCircuit();

        app.MapHealthChecks("/health/liveness", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("liveness"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).ShortCircuit();

        app.MapHealthChecks("/health/readiness", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("readiness"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).ShortCircuit();

        return app;
    }
}

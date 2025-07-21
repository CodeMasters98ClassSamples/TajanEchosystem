using Tajan.Captcha.API.Contracts;

namespace Tajan.Captcha.API.Extensions;

public static class CaptchaEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/users");

        //group.MapGet("/{id:int}", GetUser);
        group.MapGet("/Generate", Generate).WithName("GenerateCaptcha").WithOpenApi();

        return endpoints;
    }

    static IResult Generate(int id, ICaptchaService captchaService)
    {
        var captcha = captchaService.Generate();
        return captcha is null ? Results.NotFound() : Results.Ok(captcha);
    }

}
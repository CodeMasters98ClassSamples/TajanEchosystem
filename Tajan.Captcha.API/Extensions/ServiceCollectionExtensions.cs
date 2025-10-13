using Tajan.Captcha.API.CaptchaProvider;
using Tajan.Captcha.API.Contracts;
//using WonderfulCaptcha;
//using WonderfulCaptcha.Cache.InMemory;

namespace Tajan.Captcha.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaptcha(this IServiceCollection services, IConfiguration configuration)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();
        IDynamicSettingService dss = scope.ServiceProvider.GetRequiredService<IDynamicSettingService>();
        bool useRedis = dss.Get<bool>(DynamicSettingKey.CAPTCHA_USE_REDIS).Result;

        //services.AddWonderfulCaptcha(options =>
        //{
        //    options.TextOptions.TextColor = ColorEnum.Blue;
        //    options.ImageOptions.ImageBackgroundColor = ColorEnum.White;
        //    options.NoiseOptions.OilPaintLevel = 0;
        //    options.NoiseOptions.SaltAndPepperDensityPercent = 1;
        //    options.CacheOptions.CacheExpirationTime = TimeSpan.FromMinutes(10);

        //    if (useRedis)
        //        options.UseCustomCacheProvider<WonderfulCaptchaCacheProvider>();
        //    else
        //    {
        //        services.AddMemoryCache();
        //        options.UseInMemoryCacheProvider();
        //    }

        //});

        //services.AddScoped<ICaptchaProvider, WonderfulCaptchaProvider>();
        return services;
    }
}

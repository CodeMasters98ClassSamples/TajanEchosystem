using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tajan.Identity.Application.Contracts;
using Tajan.Identity.Infrastructure.Contexts;
using Tajan.Identity.Infrastructure.Models;
using Tajan.Identity.Infrastructure.Services;
using Tajan.Identity.Infrastructure.Settings;
using Tajan.Standard.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using EFCoreSecondLevelCacheInterceptor;

namespace Tajan.Identity.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {


        bool useInMemoryDb = configuration.GetValue<bool>("UseInMemoryDataBase");

        services.AddMemoryCache();
        if (useInMemoryDb)
        {
            services.AddDbContext<IApplicationDbContext, IdentityContext>(options =>
                options.UseInMemoryDatabase("PD_Identity"));
        }
        else
        {
            services.AddDbContext<IApplicationDbContext, IdentityContext>((serviceProvider, options) =>
                options.UseSqlServer(
                configuration.GetConnectionString("PD_Identity"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName))
                .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));

        }

        services.AddEFSecondLevelCache(options =>
               options.UseMemoryCacheProvider().ConfigureLogging(true).UseCacheKeyPrefix("Identity_")
                .CacheAllQueries(CacheExpirationMode.Absolute, TimeSpan.FromHours(24))
                .UseDbCallsIfCachingProviderIsDown(TimeSpan.FromMinutes(1)));

        var passwordPolicy = new PasswordPolicy();
        configuration.GetSection("PasswordPolicy").Bind(passwordPolicy);

        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = passwordPolicy.RequiredLength;
                options.Password.RequireDigit = passwordPolicy.RequireDigit;
                options.Password.RequireLowercase = passwordPolicy.RequireLowercase;
                options.Password.RequireUppercase = passwordPolicy.RequireUppercase;
                options.Password.RequireNonAlphanumeric = passwordPolicy.RequireNonAlphanumeric;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        #region Services
        services.AddTransient<IAccountService, AccountService>();
        #endregion

        services.Configure<JwtSetting>(configuration.GetSection("JwtSetting"));


        return services;
    }
}

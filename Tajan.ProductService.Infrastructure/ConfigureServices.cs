using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tajan.ProductService.API.Contracts;
using Tajan.ProductService.API.Services;
using Tajan.ProductService.Infrastructure.DbContexts;

namespace Tajan.ProductService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastrauctureLayer(this IServiceCollection services,string connecttionString)
    {
        services.AddTransient<IProductService, ProductBusiness>();

        //configuration.GetConnectionString("ApplicationDbContext")
        services.AddDbContext<CoreDbContext>((options) =>
        {
            options.UseSqlServer(connecttionString);
        }).AddHealthChecks();


        return services;
    }
}


using Tajan.ProductService.gRPCApi.Services;
using Tajan.Standard.Infrastructure.CacheProvider;
using Tajan.Standard.Presentation.Correlation;
using Tajan.Standard.Presentation.Cors;
using Tajan.ProductService.Infrastructure;
using Tajan.ProductService.Application;
using Tajan.Standard.Presentation.GlobalExceptionHandler;
using Tajan.ProductService.Infrastructure.DbContexts;
using Tajan.ProductService.API.Entities;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Tajan_Product_Db");
var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

builder.Services
    .AddGlobalExceptionHandler()
    .AddBasicCors()
    .AddCorrelationContext()
    .AddCacheProvider(builder.Configuration)
    .AddApplicationLayer()
    .AddInfrastrauctureLayer(connectionString, useInMemoryDatabase);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app
    .UseCorrelationId()
    .UseExceptionHandler()
    .UseBasicCors();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

if (app.Environment.IsDevelopment() && useInMemoryDatabase)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
        context.Products.AddRange(
                Product.Create(1, "Notebook", 5000),
                Product.Create(2, "Pen", 1000)
        );
        context.SaveChanges();
    }
}

app.Run();

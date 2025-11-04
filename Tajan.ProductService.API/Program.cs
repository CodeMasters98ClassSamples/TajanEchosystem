using Tajan.ProductService.API.Settings;
using Tajan.ProductService.Infrastructure;
using Tajan.ProductService.Application;
using Tajan.ProductService.Infrastructure.DbContexts;
using Tajan.Standard.Infrastructure.CacheProvider;
using Tajan.Standard.Presentation.Extentions;
using Tajan.Standard.Presentation.Correlation;
using Tajan.Standard.Presentation.Cors;
using Tajan.ProductService.API.Entities;
using Tajan.Standard.Presentation.GlobalExceptionHandler;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("CoreDbContext");
var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
builder.Services.Configure<MySettings>(builder.Configuration.GetSection(nameof(MySettings)));

builder.Services.AddControllers();

builder.Services
    .AddApplicationLayer()
    .AddInfrastrauctureLayer(connectionString, useInMemoryDatabase)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddGlobalExceptionHandler()
    .AddBasicCors()
    .AddCorrelationContext()
    .AddCacheProvider(builder.Configuration);

var app = builder.Build();

app
    .UseCorrelationId()
    .UseExceptionHandler()
    .UseBasicCors();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthCheckEndpoints();

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

// If running with a real SQL Server (not in-memory), ensure the DB schema/tables exist
if (!useInMemoryDatabase)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
        try
        {
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            // In local/dev docker-compose runs multiple services may attempt DB creation concurrently.
            // Ignore errors where the database already exists and continue running so the service stays up.
            Console.WriteLine($"Warning: EnsureCreated failed: {ex.Message}");
        }
    }
}
app.Run();

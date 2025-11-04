using Tajan.OrderService.Application;
using Tajan.OrderService.Infrastructure;
using Tajan.OrderService.API.Settings;
using Tajan.Standard.Infrastructure.CacheProvider;
using Tajan.OrderService.Infrastructure.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceAddresses>(builder.Configuration.GetSection(nameof(ServiceAddresses)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCacheProvider(builder.Configuration);

builder.Services
    .AddApplicationLayer()
    .AddInfrastructureLayer(configuration: builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure database schema exists when not using the in-memory provider
var useInMemory = app.Configuration.GetValue<bool>("UseInMemoryDataBase");
if (!useInMemory)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // EnsureCreated is fine for local/dev environments (creates tables for the model)
        db.Database.EnsureCreated();
    }
}

app.Run();

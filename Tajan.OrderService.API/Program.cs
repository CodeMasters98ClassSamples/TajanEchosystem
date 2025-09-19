using Tajan.OrderService.Application;
using Tajan.OrderService.Infrastructure;
using Tajan.OrderService.API.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceAddresses>(builder.Configuration.GetSection(nameof(ServiceAddresses)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

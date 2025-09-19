using MassTransit;
using MassTransit.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tajan.Notification.API.Jobs;
using Tajan.Notification.API.Persistence.Contexts;
using Tajan.Notification.API.QueueListener;
using Tajan.Standard.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseInMemoryDatabase("AppDbContext"));

builder.Services.AddHostedService<SmsOutBoxJob>();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SendSingleSmsConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        RabbitMqSettings rabbitMqSettings = ctx.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

        cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port.ToString(), rabbitMqSettings.VirtualHost, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        cfg.ReceiveEndpoint("sms-send-queue", e =>
        {
            e.UseRawJsonDeserializer(isDefault: true);

            e.ConfigureConsumer<SendSingleSmsConsumer>(ctx);
        });
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Notification API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root (اختیاری)
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

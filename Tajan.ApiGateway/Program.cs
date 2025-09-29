using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Tajan.ApiGateway.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string ocelotFile = "ocelot.json";

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (!string.IsNullOrEmpty(environment))
    ocelotFile = $"ocelot.{environment}.json";

builder.Configuration.AddJsonFile(ocelotFile, optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddCustomAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseUserIdHeader();

app.MapControllers();

await app.UseOcelot();

app.Run();

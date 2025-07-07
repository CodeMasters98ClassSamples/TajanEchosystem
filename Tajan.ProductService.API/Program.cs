using System.Runtime;
using Tajan.ProductService.API.Contracts;
using Tajan.ProductService.API.Settings;
using Tajan.ProductService.Infrastructure;
using Tajan.ProductService.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




string connectionString = builder.Configuration.GetConnectionString("CoreDbContext");
if (string.IsNullOrEmpty(connectionString))
{

}

var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");


//Check If Vairiable Set

builder.Services
    .AddApplicationLayer()
    .AddInfrastrauctureLayer(connectionString, useInMemoryDatabase);


builder.Services.Configure<MySettings>(builder.Configuration.GetSection(nameof(MySettings)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Products.AddRange(
        new Product { Name = "Notebook", Price = 5.99m },
        new Product { Name = "Pen", Price = 1.49m }
    );
    context.SaveChanges();
}


app.Run();

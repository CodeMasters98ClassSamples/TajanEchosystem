using System.Runtime;
using Tajan.ProductService.API.Contracts;
using Tajan.ProductService.API.Settings;
using Tajan.ProductService.Infrastructure;
using Tajan.ProductService.Application;
using Tajan.ProductService.Infrastructure.DbContexts;
using Tajan.ProductService.API.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("CoreDbContext");
if (string.IsNullOrEmpty(connectionString))
{

}

var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

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
    var context = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
    //context.Products.AddRange(
    //    new Product { Id = 1, Name = "Notebook", Price = 5.99m },
    //    new Product { Id = 2, Name = "Pen", Price = 1.49m }
    //);
    context.SaveChanges();
}

app.Run();

using MassTransit;
using Microsoft.AspNetCore.Identity;
using Tajan.Identity.Application;
using Tajan.Identity.Infrastructure;
using Tajan.Identity.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddMassTransit(x =>
//{
//    x.UsingRabbitMq();
//});

builder.Services
    .AddApplicationLayer()
    .RegisterInfrastructureIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var user = new ApplicationUser
    {
        Email = "darvishiparham14@gmail.com",
        FirstName = "parham",
        LastName = "darvishi",
        UserName = "darvishiparham14@gmail.com",
        PhoneNumber = "989129564205",
        PhoneNumberConfirmed = false
    };
    var result = await _userManager.CreateAsync(user, "Gg@123456$");
    await _roleManager.CreateAsync(new IdentityRole()
    {
        Name = "USER",
        NormalizedName = "USER"
    });
    await _userManager.AddToRoleAsync(user, "USER");
}


app.Run();

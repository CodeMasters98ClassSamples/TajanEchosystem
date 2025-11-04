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
    var loggerFactory = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("Startup");

    // Retry EnsureCreated a few times in case SQL Server isn't ready
    var db = scope.ServiceProvider.GetRequiredService<Tajan.Identity.Infrastructure.Contexts.IdentityContext>();
    var ensureAttempts = 0;
    var ensured = false;
    while (ensureAttempts < 6 && !ensured)
    {
        try
        {
            await db.Database.EnsureCreatedAsync();
            logger.LogInformation("Ensured Identity database is created.");
            ensured = true;
        }
        catch (Exception ex)
        {
            ensureAttempts++;
            logger.LogWarning(ex, "Attempt {Attempt} - Identity DB not ready yet. Retrying in 2s...", ensureAttempts);
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }

    try
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

        var existing = await _userManager.FindByNameAsync(user.UserName);
        if (existing == null)
        {
            var result = await _userManager.CreateAsync(user, "Gg@123456$");
            if (result.Succeeded)
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "USER",
                    NormalizedName = "USER"
                });
                await _userManager.AddToRoleAsync(user, "USER");
                logger.LogInformation("Seeded default identity user and role.");
            }
            else
            {
                logger.LogWarning("Failed to create seed user: {Errors}", string.Join(';', result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            logger.LogInformation("Seed user already exists, skipping.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while seeding identity users/roles. Continuing startup without seeding.");
    }
}


app.Run();

using Microsoft.EntityFrameworkCore;
using Tajan.Notification.API.Persistence.Contexts;

namespace Tajan.Notification.API.Jobs;

public class SmsOutBoxJob : BackgroundService
{
    private readonly ILogger<SmsOutBoxJob> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    public SmsOutBoxJob(
        IServiceScopeFactory scopeFactory,
        ILogger<SmsOutBoxJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Job started at {DateTime.Now}");
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var allSms = await db.SmsOutbox.ToListAsync();
                //call business for allSms
            }
            catch (Exception ex)
            {

            }
            _logger.LogInformation($"Job ended at {DateTime.Now}");
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}

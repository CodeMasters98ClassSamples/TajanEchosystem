using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tajan.Notification.API.Models;
using Tajan.Notification.API.Persistence.Contexts;
using Tajan.Standard.Application.ServiceIngtegrations.NotificationService;

namespace Tajan.Notification.API.QueueListener;

class SendSingleSmsConsumer : IConsumer<SendSingleSms>
{
    ////Service
    private readonly ApplicationDbContext _context;
    public SendSingleSmsConsumer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<SendSingleSms> context)
    {
        try
        {
            if (context.Message.ExternalId is not null)
            {
                if (await _context.Sms.AnyAsync(x => x.ExternalId == context.Message.ExternalId))
                    return;
            }
            Sms sms = new()
            {
                Content = context.Message.Content,
                ExternalId = context.Message.ExternalId,
                Reciever = context.Message.MobileNumber,
                Title = context.Message.Title,
            };
            await _context.Sms.AddAsync(sms);

            //_smsService.Send(); ex: Kavenegar,Sms.ir ,....
        }
        catch (Exception ex)
        {
            //outbox
            await _context.SmsOutbox.AddAsync(new SmsOutbox()
            {
                Content = JsonSerializer.Serialize(context.Message),
                CreatedAt = DateTime.UtcNow,
            });
            await _context.SaveChangesAsync();
            //Log to elastic
        }
    }
}

namespace Tajan.Standard.Application.ServiceIngtegrations.NotificationService;

public interface SendSingleSms
{
    string MobileNumber { get; set; }
    string Content { get; set; }
    string Title { get; set; }
    long? ExternalId { get; set; }
}

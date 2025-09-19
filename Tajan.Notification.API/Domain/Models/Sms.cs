namespace Tajan.Notification.API.Models
{
    public class Sms
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Reciever { get; set; }
        public long? ExternalId { get; set; }
    }
}

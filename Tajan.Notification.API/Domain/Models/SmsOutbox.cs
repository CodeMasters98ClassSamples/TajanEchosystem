namespace Tajan.Notification.API.Models
{
    public class SmsOutbox
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

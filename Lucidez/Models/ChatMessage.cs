namespace Lucidez.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string ConnectionId { get; set; } = "";

        public string Sender { get; set; } = ""; // user / admin

        public string Message { get; set; } = "";

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}

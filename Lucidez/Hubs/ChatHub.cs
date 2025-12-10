using Microsoft.AspNetCore.SignalR;
using Lucidez.Data;
using Lucidez.Models;

namespace Lucidez.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;

        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendMessage(string sender, string message)
        {
            var chat = new ChatMessage
            {
                ConnectionId = Context.ConnectionId,
                Sender = sender,
                Message = message
            };

            _db.ChatMessages.Add(chat);
            await _db.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", new
            {
                id = chat.Id,
                sender = chat.Sender,
                message = chat.Message,
                sentAt = chat.SentAt.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TodoApi.Models;

namespace TodoApi.Hubs
{
    public class ChatHub : Hub
    {
        private static List<Message> _messages = new List<Message>();

        public async Task SendMessage(string messageContent)
        {
            var newMessage = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Content = messageContent,
                Timestamp = DateTime.UtcNow
            };

            _messages.Add(newMessage);
            await Clients.All.SendAsync("ReceiveMessage", newMessage);
            await SendAllMessages();
        }

        public async Task DeleteMessage(string messageId)
        {
            var message = _messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                _messages.Remove(message);
                await Clients.All.SendAsync("DeleteMessage", messageId);
                await SendAllMessages();
            }
        }

        public async Task SendRecentMessages()
        {
            var recentMessages = _messages.OrderByDescending(m => m.Timestamp).Take(3).ToList();
            await Clients.Caller.SendAsync("ReceiveRecentMessages", recentMessages);
        }

        public Task<IEnumerable<Message>> GetNotifications()
        {
            return Task.FromResult<IEnumerable<Message>>(_messages);
        }

        public async Task SendAllMessages()
        {
            await Clients.All.SendAsync("ReceiveAllMessages", _messages);
        }

        public override async Task OnConnectedAsync()
        {
            await SendRecentMessages();
            await base.OnConnectedAsync();
        }
    }
}
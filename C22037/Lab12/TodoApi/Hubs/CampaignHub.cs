using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TodoApi.Database;
using TodoApi.Models;

namespace TodoApi.Hubs
{
    public class CampaignHub : Hub
    {
        private static List<Message> _messages = new List<Message>();
        private readonly StoreDB _storeDB;

        public CampaignHub(StoreDB storeDB)
        {
            _storeDB = storeDB;
        }

        public async Task SendMessageAsync(string messageContent)
        {
            var newMessage = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Content = messageContent,
                Timestamp = DateTime.UtcNow
            };

            _messages.Add(newMessage);
            await _storeDB.InsertMessageAsync(newMessage);
            await Clients.All.SendAsync("ReceiveMessageAsync", newMessage);
            await SendAllMessagesAsync();
        }

        public async Task DeleteMessageAsync(string messageId)
        {
            var message = _messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                await _storeDB.MarkMessageAsDeletedAsync(messageId);
                _messages.Remove(message);
                await Clients.All.SendAsync("DeleteMessageAsync", messageId);
                await SendAllMessagesAsync();
            }
        }


        public async Task SendRecentMessagesAsync()
        {
            var recentMessages = _messages.OrderByDescending(m => m.Timestamp).Take(3).ToList();
            await Clients.Caller.SendAsync("ReceiveRecentMessagesAsync", recentMessages);
        }

        public Task<IEnumerable<Message>> GetNotificationsAsync()
        {
            return Task.FromResult<IEnumerable<Message>>(_messages);
        }

        public async Task SendAllMessagesAsync()
        {
            await Clients.All.SendAsync("ReceiveAllMessagesAsync", _messages);
        }

        public override async Task OnConnectedAsync()
        {
            await SendRecentMessagesAsync();
            await base.OnConnectedAsync();
        }
    }
}
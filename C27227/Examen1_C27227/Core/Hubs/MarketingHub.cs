using KEStoreApi.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Core
{
    public class MarketingHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Content) || string.IsNullOrWhiteSpace(message.Title))
            {
                throw new ArgumentException("Invalid message: Message, title and content must not be null or empty.");
            }

            try
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                throw new HubException("An error occurred while sending the message.", ex);
            }
        }

        public async Task RemoveMessage(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid message id: Id must be a positive integer.");
            }

            try
            {
                await Clients.All.SendAsync("RemoveMessage", id);
            }
            catch (Exception ex)
            {
                throw new HubException("An error occurred while removing the message.", ex);
            }
        }
    }
}

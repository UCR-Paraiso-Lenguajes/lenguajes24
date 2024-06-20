using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using storeapi.Database;
using storeapi.Models;

namespace storeapi.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task NewCampaign(string contenidoHtml)
        {
            // Insert the campaign into the database
            Campanna newCampanna = new Campanna { ContenidoHtml = contenidoHtml };
            CampannaDB.InsertCampanna(newCampanna);
            
            // Notify all clients about the new campaign
            await Clients.All.SendAsync("UpdateCampaigns", contenidoHtml);
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using StoreAPI.Database;
using StoreAPI.models;
using System.Threading.Tasks;

namespace StoreAPI.Hubs
{

    public class CampaignHub : Hub
    {
        public async Task SendCampaignUpdate(string user, string update)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, update);
        }

        public async Task NewCampaign(string message)
        {
            // Insert the campaign into the database
            Campaign newCampaign  = new Campaign { Update = message };
            var campaignDB = new CampaignDB(); // Create an instance of CampaignDB
            await campaignDB.SaveAsync(newCampaign ); // Call SaveAsync on the instance

            // Notify all clients about the new campaign
            await Clients.All.SendAsync("UpdateCampaigns", message);
        }
    }
}




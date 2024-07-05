using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace KEStoreApi.Hubs
{
    public class CampaignHub : Hub
    {
        public async Task BroadcastNewCampaign(string campaignContent, string campaignTitle, int campaignId)
        {
            await Clients.All.SendAsync("ReceiveNewCampaign", campaignContent, campaignTitle, campaignId);
        }

        public async Task BroadcastDeletedCampaign(int campaignId)
        {
            await Clients.All.SendAsync("ReceiveDeletedCampaign", campaignId);
        }
    }
}

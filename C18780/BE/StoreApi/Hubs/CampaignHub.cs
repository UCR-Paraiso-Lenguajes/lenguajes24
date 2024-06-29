using Microsoft.AspNetCore.SignalR;
using StoreApi.Models;

namespace SignalRWebpack.Hubs;

public class CampaignHub : Hub
{
    public async Task AdCreated(Ad ad) =>
        await Clients.All.SendAsync("AdCreated", ad);
}
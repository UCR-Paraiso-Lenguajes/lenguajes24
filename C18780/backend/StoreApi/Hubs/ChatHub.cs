using Microsoft.AspNetCore.SignalR;
using StoreApi.Models;

namespace SignalRWebpack.Hubs;

public class ChatHub : Hub
{
    public async Task AdCreated(Ad ad) =>
        await Clients.All.SendAsync("AdCreated", ad);
}
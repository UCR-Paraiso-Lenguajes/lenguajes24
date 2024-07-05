using Microsoft.AspNetCore.SignalR;
using storeApi.db;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CampaignController : Hub
{
    private static List<Dictionary<string, object>> messages = new List<Dictionary<string, object>>();

    public CampaignController()
    {
        InitializeMessages().Wait();
    }

    private async Task InitializeMessages()
    {
        var initialMessages = await StoreDB.GetLastThreeMessagesAsync();
        messages = initialMessages;
    }

    public async Task SendCampaignMessage(int messageId, string messageContent)
    {
        var message = new Dictionary<string, object>
        {
            { "id", messageId },
            { "content", messageContent }
        };

        messages.Add(message);
        await UpdateAllClients();
    }

    public async Task DeleteMessage(int messageId)
    {
        messages.RemoveAll(m => (int)m["id"] == messageId);
        await UpdateAllClients();
    }

    private async Task UpdateAllClients()
    {
        await Clients.All.SendAsync("UpdateMessages", messages);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("UpdateMessages", messages);
        await base.OnConnectedAsync();
    }
}

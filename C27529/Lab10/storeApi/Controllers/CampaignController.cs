using Microsoft.AspNetCore.SignalR;
using storeApi.db;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CampaignController : Hub
{
    private static List<Dictionary<string, object>> lastThreeMessages = new List<Dictionary<string, object>>();

    public CampaignController()
    {
        InitializeLastThreeMessages().Wait();
    }

    private async Task InitializeLastThreeMessages()
    {
        var messages = await StoreDB.GetLastThreeMessagesAsync();
        lastThreeMessages = messages;
    }

    public async Task SendCampaignMessage(string content)
    {
        System.Console.WriteLine("SendCampaignMessage con el contenido: " + content);

        // Agregar el mensaje a la base de datos y obtener su ID
        int messageId = await StoreDB.AddMessageAsync(content);

        var message = new Dictionary<string, object>
    {
        { "id", messageId },
        { "content", content }
    };

        // Agregar el mensaje a la lista
        if (lastThreeMessages.Count >= 3)
        {
            lastThreeMessages.RemoveAt(0);
        }
        lastThreeMessages.Add(message);

        await UpdateAllClients();
    }

    
    public async Task DeleteMessage(int id)
    {
        lastThreeMessages.RemoveAll(m => (int)m["id"] == id);
        // No eliminamos de la base de datos
        await UpdateAllClients();
    }

    private async Task UpdateAllClients()
    {
        await Clients.All.SendAsync("UpdateMessages", lastThreeMessages);
    }

    public override async Task OnConnectedAsync()
    {
        await UpdateAllClients();
        await base.OnConnectedAsync();
    }
}

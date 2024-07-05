using System.Collections.Generic;
using System.Text.Json;
using Fleck;
using Microsoft.Extensions.Logging;

namespace ApiLab7;

public class WebSocketService
{
    private WebSocketServer _server;
    private List<IWebSocketConnection> _wsConnections = new List<IWebSocketConnection>();
    private CampaignData _campaignData;

    public WebSocketService(string url)
    {
        _server = new WebSocketServer(url);
        _campaignData = new CampaignData();

        _server.Start(ws =>
        {
            ws.OnOpen = async () =>
            {
                _wsConnections.Add(ws);
                await SendInitialCampaigns(ws);
            };

            ws.OnClose = () =>
            {
                _wsConnections.Remove(ws);
            };

            ws.OnMessage = message =>
            {
                BroadcastMessage(message);
            };
        });
    }

    public void BroadcastMessage(string message)
    {
        foreach (var webSocketConnection in _wsConnections)
        {
            webSocketConnection.Send(message);
        }
    }

    private async Task SendInitialCampaigns(IWebSocketConnection ws)
    {
        var lastCampaigns = await _campaignData.GetLastThreeCampaignsAsync();
        foreach (var campaign in lastCampaigns)
        {
            var jsonCampaign = JsonSerializer.Serialize(campaign);
            ws.Send(jsonCampaign);
        }
    }

    public void SendMessage<T>(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        BroadcastMessage(jsonMessage);
    }
}

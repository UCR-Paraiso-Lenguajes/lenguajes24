using System.Collections.Generic;
using Fleck;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ApiLab7;

public class WebSocketService
{
    private WebSocketServer _server;
    private List<IWebSocketConnection> _wsConnections = new List<IWebSocketConnection>();
    private MessageData _messageData;

    public WebSocketService(string url)
    {
        _server = new WebSocketServer(url);
        _messageData = new MessageData();

        _server.Start(ws =>
        {
            ws.OnOpen = async () =>
            {
                _wsConnections.Add(ws);
                await SendInitialMessages(ws);
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

    private async Task SendInitialMessages(IWebSocketConnection ws)
    {
        var lastMessages = await _messageData.GetLastThreeMessagesAsync();
        foreach (var message in lastMessages)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            ws.Send(jsonMessage);
        }
    }

    public void SendMessage<T>(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        BroadcastMessage(jsonMessage);
    }
}

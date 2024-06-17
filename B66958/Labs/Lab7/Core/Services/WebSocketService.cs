using System.Collections.Generic;
using Fleck;

namespace ApiLab7;

public class WebSocketService
{
    private WebSocketServer _server;
    private List<IWebSocketConnection> _wsConnections = new List<IWebSocketConnection>();

    public WebSocketService(string url)
    {
        _server = new WebSocketServer(url);

        _server.Start(ws =>
        {
            ws.OnOpen = () =>
            {
                _wsConnections.Add(ws);
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

    public void SendMessage(string message)
    {
        BroadcastMessage(message);
    }
}

using System.Net.WebSockets;

internal class WebsocketList
{
    public List<WebSocket> _connections;

    public WebsocketList()
	{
        _connections = new List<WebSocket>();
    }
}
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public interface IWSConnectionHandler {
        // Middleware API
        void AddController(IWSController controller);
        Task OnConnected(WebSocket socket);
        Task OnDisconnected(WebSocket socket);
        Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

        // Controller API
        Task SendMessage(string message, Func<WSConnection, bool> filter);
        Task SendMessage(WSConnection connection, string message);
        Task DropConnection(WSConnection connection, WebSocketCloseStatus closeStatus, string reason);
        Task DropConnections(WebSocketCloseStatus closeStatus, string reason, Func<WSConnection, bool> filter);
    }
}

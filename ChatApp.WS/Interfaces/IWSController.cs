using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public interface IWSController {

        void SetConnectionHandler(WSConnectionHandler handler);

        // controller API listeners & handler API
        Task OnConnected(WSConnection connection);
        Task OnDisconnected(WSConnection connection);

        Task OnMessage(WSConnection connection, string message);

        // controller API
        Task SendMessage(WSConnection connection, string message);
        Task SendMessage(string message, Func<WSConnection, bool> filter = null);

        Task DropConnection(WSConnection connection, WebSocketCloseStatus closeStatus, string reason);
        Task DropConnections(WebSocketCloseStatus closeStatus, string reason, Func<WSConnection, bool> filter = null);
    }
}

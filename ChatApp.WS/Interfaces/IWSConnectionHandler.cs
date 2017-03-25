using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public interface IWSConnectionHandler {
        void AddController(IWSController controller);
        Task OnConnected(WebSocket socket);
        Task OnDisconnected(WebSocket socket);
        Task SendMessageAsync(string message, Func<WSConnection, bool> filter);
        Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}

using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public interface IWSConnectionHandler {
        void AddController(IWSController controller);
        Task OnConnected(WebSocket socket);
        Task OnDisconnected(WebSocket socket);
        Task SendMessageToAllAsync(string message);
        Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}

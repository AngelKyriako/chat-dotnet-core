using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public interface IWSController {

        void SetConnectionHandler(WSConnectionHandler handler);

        // listeners
        Task OnConnected(WSConnection connection);
        Task OnDisconnected(WSConnection connection);
        Task OnMessage(WSConnection connection, string message);
    }
}

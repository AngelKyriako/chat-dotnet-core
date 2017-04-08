using System.Net.WebSockets;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace ChatApp.Web.Controllers {
    using Auth;
    using Model;
    using WS;

    public class SocketController: WSControllerGeneric<WSMessage<MessageModel>>  {

        private ILogger _logger;
        private IAuthService _auth;

        public SocketController(ILoggerFactory loggerFactory, IAuthService auth) {
            _logger = loggerFactory.CreateLogger<SocketController>();
            _auth = auth;
        }

        //public override async Task OnConnected(WSConnection connection) {
        //    await SendMessage(connection + " connected");
        //}

        //public override async Task OnDisconnected(WSConnection connection) {
        //    await SendMessage(connection + " disconnected");
        //}

        public override async Task OnMessageSerialized(WSConnection connection, string message) {
            _logger.LogInformation(connection + " sent: " + message);
            //await SendMessage(connection + " sent: " + message, (conn) => {
            //    return (conn.Id == connection.Id);
            //});
        }

        public override async Task OnMessageDeserialized(WSConnection connection, WSMessage<MessageModel> message) {
            _logger.LogInformation("accessToken: " + message.AccessToken);

            UserModel authedUser = _auth.DecodeTokenToUser(message.AccessToken);
            if (authedUser == null) {
                await DropConnection(connection, WebSocketCloseStatus.InvalidPayloadData, "authorization failed");
                return;
            }
        }
    }
}

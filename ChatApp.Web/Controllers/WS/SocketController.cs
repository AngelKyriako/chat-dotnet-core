using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace ChatApp.Web.Controllers {
    using Auth;
    using Model;
    using WS;

    public class SocketController: WSController  {

        private ILogger _logger;
        private IAuthService _auth;

        public SocketController(ILoggerFactory loggerFactory, IAuthService auth) {
            _logger = loggerFactory.CreateLogger<SocketController>();
            _auth = auth;
        }

        public override async Task OnConnected(WSConnection connection) {
            _logger.LogInformation(connection + " connected");
        }

        public override async Task OnDisconnected(WSConnection connection) {
            _logger.LogInformation(connection + "disconnected");
        }

        public override async Task OnMessage(WSConnection connection, string message) {
            _logger.LogInformation(connection + " sent: " + message);

            //TODO: Message data structure
            //TODO: Handle authorization
            string accessToken = "";
            _logger.LogInformation("accessToken: " + accessToken);
            UserModel authedUser = _auth.DecodeTokenToUser(accessToken);
            if (authedUser == null) {
                //disconnect
                return;
            } else {
                _logger.LogInformation("authedUser: " + authedUser.ToJson());
            }

            await SendMessageToAll(connection + " sent: " + message, (conn) => {
                return (conn.Id == connection.Id);
            });
        }
    }
}

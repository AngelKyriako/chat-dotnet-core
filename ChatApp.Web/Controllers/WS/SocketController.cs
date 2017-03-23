using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace ChatApp.Web.Controllers {
    using WS;

    public class SocketController: WSController  {

        private ILogger _logger;

        public SocketController(ILoggerFactory loggerFactory) {
            _logger = loggerFactory.CreateLogger<SocketController>();
        }

        public override async Task OnConnected(WSConnection connection) {
            await base.OnConnected(connection);
            _logger.LogInformation(connection + " connected");
        }

        public override async Task OnDisconnected(WSConnection connection) {
            await base.OnDisconnected(connection);
            _logger.LogInformation(connection + "disconnected");
        }

        public override async Task OnMessage(WSConnection connection, string message) {
            await base.OnMessage(connection, message);
            _logger.LogInformation(connection + " sent: " + message);

            await SendMessageToAll(connection + " sent: " + message);
        }
    }
}

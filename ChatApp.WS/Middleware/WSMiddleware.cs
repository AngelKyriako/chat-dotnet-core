using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace ChatApp.WS {

    /// <summary>
    /// Websocket middleware to setup in an MVC app.
    /// 
    /// Classes to start from:
    /// 1) WSConnectionHolder
    /// 2) WSConnectionHandler
    /// 3) WSController
    /// 
    /// References:
    /// 1) https://radu-matei.github.io/blog/aspnet-core-websockets-middleware/
    /// 2) http://zbrad.github.io/tools/wscore/
    /// 3) http://dotnetthoughts.net/using-websockets-in-aspnet-core/
    /// </summary>
    public class WSMiddleware {

        private const int BUFFER_SIZE = 1024 * 4;

        private readonly RequestDelegate _next;
        private WSConnectionHandler _wsConnectionHandler;

        public WSMiddleware(RequestDelegate next, WSConnectionHandler wsConnectionHandler) {
            _next = next;
            _wsConnectionHandler = wsConnectionHandler;
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> callback) {
            byte[] buffer = new byte[BUFFER_SIZE];

            while (socket.State == WebSocketState.Open) {
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                                          cancellationToken: CancellationToken.None);

                callback(result, buffer);
            }
        }

        public async Task Invoke(HttpContext context) {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await _wsConnectionHandler.OnConnected(socket);

            await Receive(socket, async (result, buffer) => {
                if (result.MessageType == WebSocketMessageType.Text) {
                    await _wsConnectionHandler.ReceiveAsync(socket, result, buffer);
                } else if (result.MessageType == WebSocketMessageType.Close) {
                    await _wsConnectionHandler.OnDisconnected(socket);
                }
                return;
            });

            // TODO - investigate the Kestrel exception thrown when this is the last middleware
            await _next.Invoke(context);
        }
    }
}

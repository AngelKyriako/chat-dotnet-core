using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public class WSController<M> : WSControllerBase where M : class {

        private WSConnectionHandler _parentHandler;

        // serialization (defaults to JSON)
        public virtual string SerializeMessage(M message) {
            return JsonSerializer.Serialize(message);
        }

        public virtual M DeserializeMessage(string message) {
            return JsonSerializer.Deserialize<M>(message);
        }

        // listening
        public virtual async Task OnMessageDeserialized(WSConnection connection, M message) {

        }

        public virtual async Task OnMessageSerialized(WSConnection connection, string message) {

        }

        public override sealed async Task OnMessage(WSConnection connection, string message) {
            await OnMessageSerialized(connection, message);

            if (typeof(M) == typeof(string)) return;

            M messageDeserialized = DeserializeMessage(message);

            if (messageDeserialized != null) {
                await OnMessageDeserialized(connection, messageDeserialized);
            }
        }

        // emitting
        public async Task SendMessage(M message, Func<WSConnection, bool> filter = null) {
            await SendMessage(SerializeMessage(message), filter);
        }

        public async Task SendMessage(WSConnection connection, M message) {
            await SendMessage(connection, SerializeMessage(message));
        }
    }
}

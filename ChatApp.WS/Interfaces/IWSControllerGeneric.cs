using System;
using System.Threading.Tasks;

namespace ChatApp.WS {

    public interface IWSControllerGeneric<M>: IWSController {        
        // controller API listeners & handler API
        Task OnMessageDeserialized(WSConnection connection, M message);
        Task OnMessageSerialized(WSConnection connection, string message);

        // controller API
        Task SendMessage(M message, Func<WSConnection, bool> filter = null);
        Task SendMessage(WSConnection connection, M message);

        string SerializeMessage(M message);
        M DeserializeMessage(string message);
    }
}
using System;

namespace ChatApp.Client {
    using Model;

    public class Api {

        public ApiUserResource User { get; private set; }
        public ApiResource<MessageModel> Message { get; private set; }

        public Api (Uri serverUrl) {
            User = new ApiUserResource(serverUrl);
            Message = new ApiResource<MessageModel>(serverUrl, "message");
        }

        public Api(string serverUrl) : this(new Uri(serverUrl)) {
        }
    }
}

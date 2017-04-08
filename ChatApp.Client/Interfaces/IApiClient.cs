using System;

namespace ChatApp.Client {
    using Model;

    public interface IApiClient {
        Uri ServerUrl { get; }
        string Version { get; }
        UserAndToken Session { get; set; }
        bool IsAuthenticated { get; }
    }
}

namespace ChatApp.Client {
    using Model;

    public interface IApiResource {
        IApiClient Parent { get; }
        UserAndToken Session { get; set; }
        string Version { get; }
    }
}

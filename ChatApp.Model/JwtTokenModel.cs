namespace ChatApp.Model {
    public class JwtTokenModel {
        public string AccessToken { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}

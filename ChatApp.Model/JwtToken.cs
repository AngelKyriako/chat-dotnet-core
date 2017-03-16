namespace ChatApp.Model {

    public class JwtToken {

        public string AccessToken { get; set; }

        public int ExpiresInSeconds { get; set; }
    }
}

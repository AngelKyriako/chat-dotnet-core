namespace ChatApp.Auth {

    public interface ICryptoMan {

        string Hash(string str);
        bool HashEqualsString(string hash, string str);
    }
}

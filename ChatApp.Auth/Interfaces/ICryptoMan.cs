namespace ChatApp.Auth {

    public interface ICryptoMan {

        byte[] GenerateSalt();
        string HashWithSalt(string str, byte[] salt);
    }
}

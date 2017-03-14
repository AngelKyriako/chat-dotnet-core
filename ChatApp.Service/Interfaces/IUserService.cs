namespace ChatApp.Service {

    using Model;

    public interface IUserService<K> : ICRUDService<UserModel<K>, K> {
        UserModel<K> GetOneByUsername(string username);
        UserModel<K> GetOneEnabledByUsername(string username);
        UserModel<K> GetOneDisabledByUsername(string username);

        bool IsValidAuthentication(UserModel<K> user, string password);
        bool IsValidAuthentication(string username, string password);
    }
}

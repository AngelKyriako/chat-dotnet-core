namespace ChatApp.Repository {
    using Model;

    public interface IUserRepository<K> : IRepository<UserModel<K>, K> {

        UserModel<K> GetOneByUsername(string username);
        UserModel<K> GetOneEnabledByUsername(string username);
        UserModel<K> GetOneDisabledByUsername(string username);
    }
}

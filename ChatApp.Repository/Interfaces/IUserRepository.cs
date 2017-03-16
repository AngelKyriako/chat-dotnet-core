namespace ChatApp.Repository {
    using Model;

    public interface IUserRepository : IRepository<UserModel> {

        UserModel GetOneByUsername(string username);
        UserModel GetOneEnabledByUsername(string username);
        UserModel GetOneDisabledByUsername(string username);
    }
}

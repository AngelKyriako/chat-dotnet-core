namespace ChatApp.Service {

    using Model;

    public interface IUserService: ICRUDService<UserModel> {
        UserModel GetOneByUsername(string username);
        UserModel GetOneEnabledByUsername(string username);
        UserModel GetOneDisabledByUsername(string username);

        bool IsValidAuthentication(UserModel user, string password);
        bool IsValidAuthentication(string username, string password);
    }
}

using System.Threading.Tasks;

namespace ChatApp.Service {
    using Model;
    
    public interface IUserService: ICRUDService<UserModel> {

        Task<UserAndToken> CreateAndAuthenticate(UserModel user);

        UserModel GetOneByUsername(string username);
        UserModel GetOneEnabledByUsername(string username);
        UserModel GetOneDisabledByUsername(string username);
    }
}

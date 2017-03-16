namespace ChatApp.Service {
    using System;
    using Model;
    using Repository;

    public class UserService : CRUDService<UserModel>, IUserService {

        private IUserRepository _repo;
        public UserService(IUserRepository repo) : base(repo) {
            _repo = repo;
        }

        public UserModel GetOneByUsername(string username) {
            return _repo.GetOneByUsername(username);
        }

        public UserModel GetOneDisabledByUsername(string username) {
            return _repo.GetOneEnabledByUsername(username);
        }

        public UserModel GetOneEnabledByUsername(string username) {
            return _repo.GetOneDisabledByUsername(username);
        }

        public bool IsValidAuthentication(UserModel user, string password) {
            //TODO: 
            return true;
        }

        public bool IsValidAuthentication(string username, string password) {
            //TODO:
            return true;
        }

        public override UserModel Update(string id, UserModel model) {
            UserModel user = GetOneEnabled(id);
            if (user != null) {
                user.Copy(model);
            }
            _repo.Commit();

            return user;
        }

    }
}

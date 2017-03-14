namespace ChatApp.Service {
    using System;
    using Model;
    using Repository;

    public class UserService<K> : CRUDService<UserModel<K>, K>, IUserService<K> {

        private IUserRepository<K> _repo;
        public UserService(IUserRepository<K> repo) : base(repo) {
            _repo = repo;
        }

        public UserModel<K> GetOneByUsername(string username) {
            return _repo.GetOneByUsername(username);
        }

        public UserModel<K> GetOneDisabledByUsername(string username) {
            return _repo.GetOneEnabledByUsername(username);
        }

        public UserModel<K> GetOneEnabledByUsername(string username) {
            return _repo.GetOneDisabledByUsername(username);
        }

        public bool IsValidAuthentication(UserModel<K> user, string password) {
            //TODO: 
            return true;
        }

        public bool IsValidAuthentication(string username, string password) {
            //TODO:
            return true;
        }

        public override void Update(K id, UserModel<K> model) {
            UserModel<K> user = GetOneEnabled(id);
            if (user != null) {
                user.Copy(model);
            }
            _repo.Commit();
        }

    }
}

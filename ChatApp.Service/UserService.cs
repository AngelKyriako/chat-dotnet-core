using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace ChatApp.Service {
    using Model;
    using Repository;
    using Auth;

    public class UserService : CRUDService<UserModel>, IUserService {

        private ILogger _logger;
        private IUserRepository _repo;
        private IAuthService _auth;

        public UserService(ILoggerFactory loggerFactory, IUserRepository repo, IAuthService auth) : base(repo) {
            _logger = loggerFactory.CreateLogger<UserService>();
            _repo = repo;
            _auth = auth;
        }

        public override UserModel Create(UserModel model) {
            model.PasswordSalt = _auth.Crypto.GenerateSalt();
            model.PasswordHash = _auth.Crypto.HashWithSalt(model.Password, model.PasswordSalt);
            model.Password = null;

            if (model.Firstname == null) {
                model.Firstname = model.Username;
            }
            if (model.Lastname == null) {
                model.Lastname = string.Empty;
            }

           return base.Create(model);
        }

        public async Task<UserAndToken> CreateAndAuthenticate(UserModel model) {
            string password = model.Password;

            UserModel createdModel = Create(model);
            return await _auth.IssueToken(createdModel, password);
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

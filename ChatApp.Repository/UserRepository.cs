
using System.Linq;

namespace ChatApp.Repository {

    using Configuration;
    using Model;

    public class UserRepository : EntityRepository<UserModel>, IUserRepository {

        public UserRepository(EntityContext context) : base(context) { }

        public UserModel GetOneByUsername(string username) {
            return _entities
                .Where(e => e.Username.Equals(username))
                .FirstOrDefault();
        }

        public UserModel GetOneDisabledByUsername(string username) {
            return _entities
                .Where(e => e.Username.Equals(username))
                .Where(e => !e.Enabled)
                .FirstOrDefault();
        }

        public UserModel GetOneEnabledByUsername(string username) {
            return _entities
                .Where(e => e.Username.Equals(username))
                .Where(e => e.Enabled)
                .FirstOrDefault();
        }
    }
}

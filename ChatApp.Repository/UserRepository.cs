
using System.Linq;

namespace ChatApp.Repository {
    using Configuration;
    using Model;

    public class UserRepository<K> : EntityRepository<UserModel<K>, K>, IUserRepository<K> {

        public UserRepository(AppEntityContext<K> context) : base(context) { }

        public UserModel<K> GetOneByUsername(string username) {
            return _entities
                .Where(e => e.Username.Equals(username))
                .FirstOrDefault();
        }

        public UserModel<K> GetOneDisabledByUsername(string username) {
            return _entities
                .Where(e => e.Username.Equals(username))
                .Where(e => !e.Enabled)
                .FirstOrDefault();
        }

        public UserModel<K> GetOneEnabledByUsername(string username) {
            return _entities
                .Where(e => e.Username.Equals(username))
                .Where(e => e.Enabled)
                .FirstOrDefault();
        }
    }
}

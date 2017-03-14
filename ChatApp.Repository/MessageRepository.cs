using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository {
    using Configuration;
    using Model;

    public class MessageRepository<K> : EntityRepository<MessageModel<K>, K>, IMessageRepository<K> {

        public MessageRepository(AppEntityContext<K> context) : base(context) { }

        public override IEnumerable<MessageModel<K>> Get(int page, int limit) {
            return _entities
                .Skip(page * limit)
                .Take(limit)
                .Include(e => e.Creator)
                .ToList();
        }

        public override IEnumerable<MessageModel<K>> GetEnabled(int page, int limit) {
            return _entities
                .Where(e => e.Enabled)
                .Skip(page * limit)
                .Take(limit)
                .Include(e => e.Creator)
                .ToList();
        }

        public override IEnumerable<MessageModel<K>> GetDisabled(int page, int limit) {
            return _entities
                .Where(e => !e.Enabled)
                .Skip(page * limit)
                .Take(limit)
                .Include(e => e.Creator)
                .ToList();
        }

        public override MessageModel<K> GetOne(K id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .Include(e => e.Creator)
                .FirstOrDefault();
        }

        public override MessageModel<K> GetOneEnabled(K id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .Where(e => e.Enabled)
                .Include(e => e.Creator)
                .FirstOrDefault();
        }

        public override MessageModel<K> GetOneDisabled(K id) {
            return _entities
                .Where(e => !e.Id.Equals(id))
                .Where(e => e.Enabled)
                .Include(e => e.Creator)
                .FirstOrDefault();
        }

    }
}

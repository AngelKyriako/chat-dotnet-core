using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository {
    using Configuration;
    using Model;

    public class MessageRepository : EntityRepository<MessageModel>, IMessageRepository {

        public MessageRepository(EntityContext context) : base(context) { }

        public override IEnumerable<MessageModel> Get(int page, int limit) {
            return _entities
                .Skip(page * limit)
                .Take(limit)
                .Include(e => e.Creator)
                .ToList();
        }

        public override IEnumerable<MessageModel> GetEnabled(int page, int limit) {
            return _entities
                .Where(e => e.Enabled)
                .Skip(page * limit)
                .Take(limit)
                .Include(e => e.Creator)
                .ToList();
        }

        public override IEnumerable<MessageModel> GetDisabled(int page, int limit) {
            return _entities
                .Where(e => !e.Enabled)
                .Skip(page * limit)
                .Take(limit)
                .Include(e => e.Creator)
                .ToList();
        }

        public override MessageModel GetOne(string id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .Include(e => e.Creator)
                .FirstOrDefault();
        }

        public override MessageModel GetOneEnabled(string id) {
            return _entities
                .Where(e => e.Id.Equals(id))
                .Where(e => e.Enabled)
                .Include(e => e.Creator)
                .FirstOrDefault();
        }

        public override MessageModel GetOneDisabled(string id) {
            return _entities
                .Where(e => !e.Id.Equals(id))
                .Where(e => e.Enabled)
                .Include(e => e.Creator)
                .FirstOrDefault();
        }

    }
}

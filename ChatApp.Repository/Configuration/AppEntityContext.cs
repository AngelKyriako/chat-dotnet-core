using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Configuration {

    using Model;

    public class AppEntityContext<K> : DbContext {
        public AppEntityContext(DbContextOptions<AppEntityContext<K>> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            new BaseEntityConstraints<K>(modelBuilder.Entity<BaseModel<K>>());
            new OwnedEntityConstraints<K>(modelBuilder.Entity<OwnedModel<K>>());
            new UserEntityConstraints<K>(modelBuilder.Entity<UserModel<K>>());
            new MessageEntityConstraints<K>(modelBuilder.Entity<MessageModel<K>>());
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Configuration {
    using Model;

    public class EntityContext : DbContext {

        public EntityContext(DbContextOptions<EntityContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            new BaseEntityConstraints(modelBuilder.Entity<BaseModel>());
            new OwnedEntityConstraints(modelBuilder.Entity<OwnedModel>());
            new UserEntityConstraints(modelBuilder.Entity<UserModel>());
            new MessageEntityConstraints(modelBuilder.Entity<MessageModel>());
        }
    }
}

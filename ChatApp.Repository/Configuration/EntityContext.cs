using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChatApp.Repository.Configuration {
    using Model;

    public class EntityContext : DbContext {

        private static bool initialized = false;

        public EntityContext(DbContextOptions<EntityContext> options) : base(options) {
            if (!initialized) {
                initialized = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            new UserEntityConstraints(modelBuilder.Entity<UserModel>());
            new MessageEntityConstraints(modelBuilder.Entity<MessageModel>());
        }

        public override int SaveChanges() {
            DateTime now = DateTime.UtcNow;

            foreach (EntityEntry entry in ChangeTracker.Entries()) {
                if (entry.State == EntityState.Added) {
                    entry.Property("CreatedAt").CurrentValue = now;
                    entry.Property("UpdatedAt").CurrentValue = now;
                    entry.Property("Enabled").CurrentValue = true;
                } else if (entry.State == EntityState.Modified) {
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
            }

            return base.SaveChanges();
        }
    }
}

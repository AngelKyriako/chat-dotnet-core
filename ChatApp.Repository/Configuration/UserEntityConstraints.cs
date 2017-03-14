using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Configuration {

    using Model;

    public class UserEntityConstraints<K> {
        public UserEntityConstraints(EntityTypeBuilder<UserModel<K>> builder) {
            //builder.HasKey(e => e.Id);

            //builder.Property(e => e.CreatedAt)
            //    //.HasValueGenerator<DateTimeNowGenerator>()
            //    .ValueGeneratedOnAdd();
            //builder.Property(e => e.UpdatedAt)
            //    //.HasValueGenerator<DateTimeNowGenerator>()
            //    .ValueGeneratedOnAddOrUpdate();

            //builder.Property(e => e.Enabled)
            //    .HasValueGenerator<BoolTrueGenerator>()
            //    .ValueGeneratedOnAdd();

            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(128)
                .IsUnicode(true);
            builder.Property(e => e.Firstname)
                .HasMaxLength(128)
                .IsUnicode(true);
            builder.Property(e => e.Lastname)
                .HasMaxLength(128)
                .IsUnicode(true);
        }
    }
}

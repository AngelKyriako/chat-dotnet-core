using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Configuration {
    using Model;

    public class UserEntityConstraints {

        public UserEntityConstraints(EntityTypeBuilder<UserModel> builder) {

            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(128)
                .IsUnicode(true);

            builder.Ignore(e => e.Password);

            builder.Property(e => e.Firstname)
                .HasMaxLength(128)
                .IsUnicode(true);

            builder.Property(e => e.Lastname)
                .HasMaxLength(128)
                .IsUnicode(true);
        }
    }
}

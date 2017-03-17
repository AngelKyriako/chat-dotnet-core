using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Configuration {
    using Model;

    public class OwnedEntityConstraints {

        public static void Configure<M>(EntityTypeBuilder<M> builder) where M : OwnedModel {
            builder.Ignore(e => e.CreatorId);

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorKey)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

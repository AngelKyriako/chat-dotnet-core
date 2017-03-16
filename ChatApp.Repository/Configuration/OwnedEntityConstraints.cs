using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Configuration {

    using Model;

    public class OwnedEntityConstraints {

        public OwnedEntityConstraints(EntityTypeBuilder<OwnedModel> builder) {

            builder.Ignore(e => e.CreatorId);

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorKey);
        }
    }
}

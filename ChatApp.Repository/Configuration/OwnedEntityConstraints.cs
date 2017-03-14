using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Configuration {

    using Model;

    public class OwnedEntityConstraints<K> {
        public OwnedEntityConstraints(EntityTypeBuilder<OwnedModel<K>> builder) {
            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Configuration {

    using Model;

    public class MessageEntityConstraints<K> {
        public MessageEntityConstraints(EntityTypeBuilder<MessageModel<K>> builder) {
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
            
            builder.Property(e => e.Body)
                .IsRequired()
                .HasMaxLength(512)
                .IsUnicode(true);
        }
    }
}

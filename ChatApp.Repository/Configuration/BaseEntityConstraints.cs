using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Configuration {
    using Model;

    public class BaseEntityConstraints {

        public static void Configure<M>(EntityTypeBuilder<M> builder) where M : BaseModel {
            builder.HasKey(e => e.Key);

            builder.Ignore(e => e.Id);
        }
    }
}
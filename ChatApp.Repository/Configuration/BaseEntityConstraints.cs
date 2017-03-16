using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace ChatApp.Repository.Configuration {

    using Model;

    public class DateTimeNowGenerator : ValueGenerator {
        public override bool GeneratesTemporaryValues { get { return false; } }

        protected override object NextValue(EntityEntry entry) {
            return DateTime.UtcNow;
        }
    }

    public class BoolTrueGenerator : ValueGenerator {
        public override bool GeneratesTemporaryValues { get { return false; } }

        protected override object NextValue(EntityEntry entry) {
            return true;
        }
    }

    public class BaseEntityConstraints {

        public BaseEntityConstraints(EntityTypeBuilder<BaseModel> builder) {
            builder.HasKey(e => e.Key);

            builder.Ignore(e => e.Id);

            builder.Property(e => e.CreatedAt)
                //.HasValueGenerator<DateTimeNowGenerator>()
                .ValueGeneratedOnAdd();
            builder.Property(e => e.UpdatedAt)
                //.HasValueGenerator<DateTimeNowGenerator>()
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(e => e.Enabled)
                .HasValueGenerator<BoolTrueGenerator>()
                .ValueGeneratedOnAdd();
        }
    }
}
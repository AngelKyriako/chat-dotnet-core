﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ChatApp.Repository.Configuration {
    using Model;

    public class MessageEntityConstraints {

        public MessageEntityConstraints(EntityTypeBuilder<MessageModel> builder) {
            builder.ToTable("message");

            BaseEntityConstraints.Configure(builder);
            OwnedEntityConstraints.Configure(builder);

            builder.Property(e => e.Body)
                .IsRequired()
                .HasMaxLength(512)
                .IsUnicode(true);
        }
    }
}

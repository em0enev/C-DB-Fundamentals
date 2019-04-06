using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VaporStore.Data.Models;

namespace VaporStore.Data.EntityConfiguration
{
    public class GameTagConfiguration : IEntityTypeConfiguration<GameTag>
    {
        public void Configure(EntityTypeBuilder<GameTag> builder)
        {
            builder.HasKey(x => new { x.GameId, x.TagId });

            builder
                .HasOne(x => x.Game)
                .WithMany(x => x.GameTags)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Tag)
                .WithMany(x => x.GameTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

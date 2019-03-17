using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.Data.EntityConfiguration
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public UserConfig()
        {
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder
                .Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            builder
                .Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            builder
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(false);

            builder
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}

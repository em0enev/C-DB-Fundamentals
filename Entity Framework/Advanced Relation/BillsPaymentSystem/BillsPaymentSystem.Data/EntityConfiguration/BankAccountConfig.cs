using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.Data.EntityConfiguration
{
    internal class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public BankAccountConfig()
        {
        }

        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(b => b.BankAccountId);

            builder.Property(b => b.BankName).HasMaxLength(50).IsUnicode();
            builder.Property(b => b.SWIFTCode).HasMaxLength(20);
        }
    }
}

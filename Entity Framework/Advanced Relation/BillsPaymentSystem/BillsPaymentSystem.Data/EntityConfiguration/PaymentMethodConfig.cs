using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.Data.EntityConfiguration
{
    internal class PaymentMethodConfig : IEntityTypeConfiguration<PaymentMethod>
    {
        public PaymentMethodConfig()
        {
        }

        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasOne(p => p.CreditCard)
                .WithOne(p => p.PaymentMethod);

            builder
                .HasOne(p => p.BankAccount)
                .WithOne(p => p.PaymentMethod);

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.PaymentMethods)
                .HasForeignKey(p => p.UserId);

        }
    }
}

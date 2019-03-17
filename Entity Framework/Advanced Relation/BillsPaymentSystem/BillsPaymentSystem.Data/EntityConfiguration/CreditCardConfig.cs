using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillsPaymentSystem.Data.EntityConfiguration
{
    internal class CreditCardConfig : IEntityTypeConfiguration<CreditCard>
    {
        public CreditCardConfig()
        {
        }

        void IEntityTypeConfiguration<CreditCard>.Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasKey(c => c.CreditCardId);
        }
    }
}
